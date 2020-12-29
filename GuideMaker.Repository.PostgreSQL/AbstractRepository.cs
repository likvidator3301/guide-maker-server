using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuideMaker.Repository.Exceptions;
using GuideMaker.Repository.Filters;
using GuideMaker.Repository.Models;
using JetBrains.Annotations;
using Npgsql;
using NpgsqlTypes;

namespace GuideMaker.Repository.PostgreSQL
{
    public abstract class AbstractRepository<T> where T : Model
    {
        private readonly NpgSqlConnectionFactory npgSqlConnectionFactory;

        protected readonly string TableName;

        protected AbstractRepository(NpgSqlConnectionFactory npgSqlConnectionFactory, string tableName)
        {
            this.npgSqlConnectionFactory = npgSqlConnectionFactory;
            TableName = tableName;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            int result;
            {
                using var connection = await npgSqlConnectionFactory.OpenAsync();
                using var command = connection.CreateCommand();

                command.CommandText = $"SELECT COUNT(*) FROM {TableName} WHERE id = @id";
                command.Parameters.Add(
                    new NpgsqlParameter("id", NpgsqlDbType.Varchar)
                    {
                        Value = id
                    });

                result = Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            return result == 1;
        }

        public async Task<T> GetAsync(string id)
        {
            T result;
            {
                using var connection = await npgSqlConnectionFactory.OpenAsync();
                using var command = connection.CreateCommand();

                command.CommandText = $"SELECT data FROM {TableName} WHERE id = @id";
                command.Parameters.Add(
                    new NpgsqlParameter("id", NpgsqlDbType.Varchar)
                    {
                        Value = id
                    });

                using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
                if (!await reader.ReadAsync())
                    throw new EntityNotFoundException($"{typeof(T).Name} '{id}'");

                result = await reader.GetFieldValueAsync<T>(0);
            }
            return result;
        }

        public async Task<T[]> SearchAsync(IFilter filter, int take, int skip)
        {
            T[] result;

            {
                var (condition, dbParameters) = filter?.ToCondition() ?? (null, Array.Empty<DbParameter>());

                using var connection = await npgSqlConnectionFactory.OpenAsync();
                using var command = connection.CreateCommand();

                var commandText = new StringBuilder()
                    .Append("SELECT coalesce(jsonb_agg(data), '[]'::jsonb) FROM ")
                    .Append("( SELECT data FROM ")
                    .Append(TableName);

                if (!string.IsNullOrWhiteSpace(condition))
                    commandText.Append(" WHERE ").Append(condition);

                commandText.Append(" LIMIT ");
                commandText.Append(take);
                commandText.Append(" OFFSET ");
                commandText.Append(skip);
                commandText.Append(") as q");

                command.CommandText = commandText.ToString();

                foreach (var dbParameter in dbParameters)
                    command.Parameters.Add(dbParameter);

                using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
                if (!await reader.ReadAsync())
                {
                    return Array.Empty<T>();
                }

                result = await reader.GetFieldValueAsync<T[]>(0);
            }

            return result;
        }

        public async Task SaveAsync(T model)
        {
            using var connection = await npgSqlConnectionFactory.OpenAsync();
            using var command = connection.CreateCommand();

            command.CommandText = $@"INSERT INTO {TableName} (id, data) VALUES (@id, @data)
                                            ON CONFLICT (id) DO UPDATE SET data = EXCLUDED.data";
            command.Parameters.Add(
                new NpgsqlParameter("id", NpgsqlDbType.Varchar)
                {
                    Value = model.Id
                });
            command.Parameters.Add(
                new NpgsqlParameter("data", NpgsqlDbType.Jsonb)
                {
                    Value = model
                });

            await command.ExecuteNonQueryAsync();
        }
    }

    public static class FilterExtension
    {
        public static (string, DbParameter[]) ToCondition([NotNull] this IFilter filter)
        {
            return filter.Type switch
            {
                FilterType.Comparison => ToCondition((ComparisonFilter) filter),
                FilterType.Logical => ToCondition((LogicalFilter) filter),
                _ => throw new ArgumentException($"Unexpected filter type {filter.Type}")
            };
        }

        private static (string, DbParameter[]) ToCondition([NotNull] this LogicalFilter filter)
        {
            var innerFilters = filter.Filters.Select(ToCondition).ToArray();
            switch (filter.Operator)
            {
                case LogicalOperator.And:
                    return ($"({string.Join(" AND ", innerFilters.Select(v => v.Item1))})",
                        innerFilters.Select(v => v.Item2).SelectMany(f => f).ToArray());

                default:
                    throw new ArgumentException($"Unexpected logical filter operator {filter.Operator}");
            }
        }

        private static (string, DbParameter[]) ToCondition([NotNull] this ComparisonFilter filter)
        {
            var fieldValue = filter.Value;

            var fieldName = filter.FieldName;

            var fieldNameParameterName = Guid.NewGuid().ToString("n").Substring(0, 12);
            var fieldValueParameterName = Guid.NewGuid().ToString("n").Substring(0, 12);

            switch (filter.Operator)
            {
                case ComparisonOperator.Equal:
                    return ($"(data->>@{fieldNameParameterName} like @{fieldValueParameterName})", new DbParameter[]
                    {
                        new NpgsqlParameter(fieldNameParameterName, fieldName),
                        new NpgsqlParameter(fieldValueParameterName, fieldValue)
                    });
                case ComparisonOperator.Contains:
                    return ($"(data->>@{fieldNameParameterName} like @{fieldValueParameterName})", new DbParameter[]
                    {
                        new NpgsqlParameter(fieldNameParameterName, fieldName),
                        new NpgsqlParameter(fieldValueParameterName, "%" + fieldValue + "%")
                    });
                default:
                    throw new ArgumentException($"Unexpected comparison filter operator {filter.Operator}");
            }
        }
    }
}
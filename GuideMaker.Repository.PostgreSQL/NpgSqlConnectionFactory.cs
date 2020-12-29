using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace GuideMaker.Repository.PostgreSQL
{
    public sealed class NpgSqlConnectionFactory
    {
        private readonly string connectionString;

        public NpgSqlConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<DbConnection> OpenAsync()
        {
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            return connection;
        }
    }
}
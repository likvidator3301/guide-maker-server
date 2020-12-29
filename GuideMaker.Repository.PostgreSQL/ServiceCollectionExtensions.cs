using System;
using GuideMaker.Repository.Repositories;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.Logging;

namespace GuideMaker.Repository.PostgreSQL
{
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPostgresRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services), "ServiceCollection must not be null");

            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration), "Configuration must not be null");

            NpgsqlConnection.GlobalTypeMapper.UseJsonNet();

            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Info, true, true);

            var connectionString = configuration["ConnectionString"];

            if (connectionString is null)
                throw new ArgumentNullException(nameof(connectionString));

            services.AddSingleton(new NpgSqlConnectionFactory(connectionString));
            services.AddSingleton<IGuideRepository, GuideRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ISlideRepository, SlideRepository>();

            return services;
        }
    }
}
using System;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Infrastructure.Extensions;
using CoreMessageBus.ServiceBus.Internal;
using CoreMessageBus.ServiceBus.SqlServer.Configuration;
using CoreMessageBus.ServiceBus.SqlServer.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreMessageBus.ServiceBus.SqlServer.Extensions
{
    public static class ServiceBusOptionsExtensions
    {
        public static ServiceBusOptions UseSqlServer(this ServiceBusOptions options,
            Action<SqlServerQueueOperationOptions> optionsAction)
        {
            var services = options.GetInfrastructure().Services;

            services.TryAdd(
                new ServiceCollection()
                    .AddSingleton<IServiceBusQueue, SqlServerServiceBusQueue>()
                    .AddSingleton<SqlQueueItemFactory>()
                    .AddSingleton<SqlQueries>()
                    .AddScoped<IDbCommandFactory, SqlDbCommandFactory>()
                    .AddScoped<ISqlConnectionFactory, SqlConnectionFactory>()
                );

            var queueOptions = new SqlServerQueueOperationOptions();
            optionsAction(queueOptions);
            services.TryAddSingleton(queueOptions);

            AddConnectionStringSource(queueOptions, services);

            return options;
        }

        private static void AddConnectionStringSource(SqlServerQueueOperationOptions options,
            IServiceCollection services)
        {
            if (options.ConnectionStringValue != null)
            {
                services.TryAddSingleton<IConnectionStringSource, DefaultConnectionStringSource>();
            }
        }
    }

}
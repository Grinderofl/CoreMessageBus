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
        public static ServiceBusOptions UseSqlServer(this ServiceBusOptions options, string connectionString = null)
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

            AddConnectionStringSource(connectionString, services);

            var queueOptions = new SqlServerQueueOperationOptions();
            services.TryAddSingleton(queueOptions);
            return options;
        }

        private static void AddConnectionStringSource(string connectionString, IServiceCollection services)
        {
            if (connectionString != null)
            {
                services.TryAddSingleton<IConnectionStringSource>(new DefaultConnectionStringSource(connectionString));
            }
        }
    }


}
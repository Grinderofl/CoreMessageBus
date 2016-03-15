using CoreMessageBus.ServiceBus;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Infrastructure.Extensions;
using CoreMessageBus.ServiceBus.Queue;
using CoreMessageBus.SqlServer.Queue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace CoreMessageBus.SqlServer.Extensions
{
    public static class ServiceBusOptionsExtensions
    {
        public static ServiceBusOptions UseSqlServer(this ServiceBusOptions options, string connectionString = null)
        {
            var services = options.GetInfrastructure().Services;

            services.TryAddEnumerable(
                new ServiceCollection()
                .AddSingleton<IQueueOperations, SqlServerQueueOperations>()
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
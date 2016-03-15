using CoreMessageBus.ServiceBus;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreMessageBus.SqlServer.Extensions
{
    public static class ServiceBusOptionsExtensions
    {
        public static ServiceBusOptions UseSqlServer(this ServiceBusOptions options, string connectionString = null)
        {
            var services = options.GetInfrastructure().Services;
            
            services.TryAddSingleton<IQueueOperations, SqlServerQueueOperations>();

            if (connectionString != null)
            {
                services.TryAddSingleton<IConnectionStringSource>(new DefaultConnectionStringSource(connectionString));
            }

            var queueOptions = new SqlServerQueueOperationOptions();
            services.TryAddSingleton(queueOptions);
            return options;
        }
    }


}
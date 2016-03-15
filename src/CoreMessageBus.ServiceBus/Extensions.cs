using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreMessageBus.ServiceBus
{
    public static class Extensions
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services,
            Action<ServiceBusOptions> optionsAction)
        {
            var options = new ServiceBusOptions();
            optionsAction(options);

            services.TryAddScoped<IServiceBus, ServiceBus>();
            services.TryAddSingleton(options.QueueOperations);

            if (!options.SendOnlyServiceBus)
            {
                services.TryAddScoped<IServiceBusClient, ServiceBusClient>();
                services.TryAddScoped<IQueueService, QueueService>();
            }

            return services;
        }
    }
}

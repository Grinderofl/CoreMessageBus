using System;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Internal;
using CoreMessageBus.ServiceBus.Queue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreMessageBus.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services,
            Action<ServiceBusOptions> optionsAction)
        {
            var options = new ServiceBusOptions(services);
            optionsAction(options);

            services.TryAddEnumerable(new ServiceCollection()
                .AddScoped<IServiceBus, ServiceBus>()
                .AddScoped<QueueSelector>()
                .AddScoped<IDateTimeProvider, DateTimeProvider>()
                .AddScoped<IIdGenerator, IdGenerator>()
                .AddScoped<IDataSerializer, JsonDataSerializer>()
                .AddScoped<QueueItemFactory>()
                );

            services.TryAddSingleton(options);
            
            services.TryAdd(ServiceDescriptor.Singleton(options.QueueOptions.GetType(), options.QueueOptions));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(QueueOptions), options.QueueOptions));
            
            if (!options.SendOnlyServiceBus)
            {
                services.TryAddScoped<IServiceBusClient, ServiceBusClient>();
                services.TryAddScoped<IQueueService, QueueService>();
            }

            return services;
        }
    }
}

using System;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreMessageBus.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services,
            Action<ServiceBusOptions> optionsAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (optionsAction == null) throw new ArgumentNullException(nameof(optionsAction));
            var options = new ServiceBusOptions(services);
            optionsAction(options);

            services.TryAdd(new ServiceCollection()
                .AddScoped<IServiceBus, ServiceBus>()
                .AddScoped<IQueueSelector, QueueSelector>()
                .AddScoped<IDateTimeProvider, DateTimeProvider>()
                .AddScoped<IIdGenerator, IdGenerator>()
                .AddScoped<IDataSerializer, JsonDataSerializer>()
                .AddScoped<IQueueItemFactory, QueueItemFactory>()
                .AddScoped<IServiceBusUnitOfWork, ServiceBusUnitOfWork>()
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

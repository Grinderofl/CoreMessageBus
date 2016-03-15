using System;
using CoreMessageBus.Configuration;
using CoreMessageBus.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMessageBus
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBus([NotNull] this IServiceCollection services, Action<MessageBusConfiguration> configurationAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var configuration = new MessageBusConfiguration();
            configurationAction(configuration);
            services.AddSingleton(configuration);
            services
                .AddScoped<MessageHandlerRegistry>(s => s.GetService<MessageBusConfiguration>().Registry)
                .AddScoped<IMessageHandlerResolver, MessageHandlerResolver>()
                .AddScoped<IMessageHandlerFactory, MessageHandlerFactory>()
                .AddScoped<IMessageBus, MessageBus>();
            
            return services;
        }
    }
}
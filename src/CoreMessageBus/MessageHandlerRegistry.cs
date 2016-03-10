using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMessageBus
{
    public class MessageHandlerRegistry
    {
        // Internal for unit testing
        internal readonly ISet<MessageHandlerRegistryItem> RegistryItems = new HashSet<MessageHandlerRegistryItem>();

        public virtual IEnumerable<Type> HandlersFor<T>()
        {
            return RegistryItems.Where(handler => handler.Handles<T>()).SelectMany(items => items.MessageHandlers);
        }

        public void RegisterHandler<T>()
        {
            RegisterHandler(typeof(T));
        }

        public void UnRegisterHandler<T>()
        {
            UnRegisterHandler(typeof (T));
        }

        public void UnRegisterHandler(Type type)
        {
            throw new NotImplementedException();
        }

        public void RegisterHandler(Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            var handlerInterfaces =
                handlerType.GetTypeInfo()
                    .ImplementedInterfaces.Where(
                        x => x.GetTypeInfo().IsGenericType && x.GetTypeInfo().GetGenericTypeDefinition() == typeof(IMessageHandler<>));
            
            foreach (var @interface in handlerInterfaces)
            {
                var messageType = @interface.GenericTypeArguments.First();

                var registryItem = RegistryItems.FirstOrDefault(item => item.Handles(messageType));
                if (registryItem == null)
                {
                    registryItem = new MessageHandlerRegistryItem(messageType);
                    RegistryItems.Add(registryItem);
                }

                if (!registryItem.MessageHandlers.Contains(handlerType))
                    registryItem.MessageHandlers.Add(handlerType);
            }
            
        }

    }

    public class MessageBusConfiguration
    {
        internal readonly MessageHandlerRegistry Registry = new MessageHandlerRegistry();

        public MessageBusConfiguration RegisterHandler<T>()
        {
            Registry.RegisterHandler<T>();
            return this;
        }

        public MessageBusConfiguration RegisterHandler(Type handlerType)
        {
            Registry.RegisterHandler(handlerType);
            return this;
        }
    }

    public static class Extensions
    {
        public static IServiceCollection AddMessageBus([NotNull] this IServiceCollection services, Action<MessageBusConfiguration> configurationAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var configuration = new MessageBusConfiguration();
            configurationAction(configuration);
            services.AddSingleton(configuration);
            services.AddScoped<MessageHandlerRegistry>(s => s.GetService<MessageBusConfiguration>().Registry);
            services
                .AddScoped<IMessageHandlerResolver, MessageHandlerResolver>()
                .AddScoped<IMessageHandlerFactory, MessageHandlerFactory>()
                .AddScoped<IMessageBus, MessageBus>();
            
            return services;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
}
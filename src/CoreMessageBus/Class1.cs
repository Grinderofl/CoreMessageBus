using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CoreMessageBus
{
    public interface IMessageBus
    {
        void Send<TMessage>(TMessage message);
        Task SendAsync<TMessage>(TMessage message);
    }

    public interface IMessage
    {
    }

    public class MessageBus : IMessageBus
    {
        private readonly IMessageHandlerResolver _resolver;

        public MessageBus([NotNull]IMessageHandlerResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            _resolver = resolver;
        }


        public void Send<TMessage>(TMessage message)
        {
            var handlers = _resolver.Resolve<TMessage>();
            foreach (var handler in handlers)
                handler.Handle(message);
        }

        // TODO: Make actually async
        public Task SendAsync<TMessage>(TMessage message)
        {
            return Task.Run(() =>
            {
                Send(message);
            });
        }
    }

    public interface IMessageHandlerResolver
    {
        IEnumerable<IMessageHandler<TMessage>> Resolve<TMessage>();
    }



    public class MessageHandlerResolver : IMessageHandlerResolver
    {
        private IMessageHandlerFactory _factory;
        private readonly MessageHandlerRegistry _registry;

        public MessageHandlerResolver([NotNull]IMessageHandlerFactory factory, MessageHandlerRegistry registry)
        {
            
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _factory = factory;
            _registry = registry;
        }

        public IEnumerable<IMessageHandler<TMessage>> Resolve<TMessage>()
        {
            var resolvedTypes = _registry.HandlersFor<TMessage>();
            foreach (var resolvedType in resolvedTypes)
            {
                var handler = _factory.Create<TMessage>(resolvedType);
                yield return handler;
            }
        }
    }

    public class MessageHandlerRegistryItem
    {

        public MessageHandlerRegistryItem(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            MessageType = type;
        }

        public Type MessageType { get; set; }
        public ISet<Type> MessageHandlers { get; set; } = new HashSet<Type>();

        public bool Handles<T>() => MessageType == typeof (T);
        public bool Handles(Type type) => MessageType == type;
    }

    public class MessageHandlerRegistryItem<TType> : MessageHandlerRegistryItem
    {
        public MessageHandlerRegistryItem() : base(typeof(TType))
        {
        }
    }

    public class MessageHandlerRegistry
    {
        // Internal for unit testing
        internal readonly ISet<MessageHandlerRegistryItem> RegistryItems = new HashSet<MessageHandlerRegistryItem>();

        public IEnumerable<Type> HandlersFor<T>()
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

    public interface IMessageHandlerFactory
    {
        IMessageHandler<TMessage> Create<TMessage>(Type resolvedType);
    }

    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}

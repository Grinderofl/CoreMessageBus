using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CoreMessageBus
{
    public class MessageHandlerResolver : IMessageHandlerResolver
    {
        private readonly IMessageHandlerFactory _factory;
        private readonly MessageHandlerRegistry _registry;

        public MessageHandlerResolver([NotNull]IMessageHandlerFactory factory, [NotNull] MessageHandlerRegistry registry)
        {
            
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
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
}
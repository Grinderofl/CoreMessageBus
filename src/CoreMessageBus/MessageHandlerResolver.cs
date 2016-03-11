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
            var resolvedHandlerTypes = _registry.HandlersFor<TMessage>();
            foreach (var resolvedHandlerType in resolvedHandlerTypes)
            {
                var handler = ResolveHandlerType<TMessage>(resolvedHandlerType);
                yield return handler;
            }
        }

        protected virtual IMessageHandler<TMessage> ResolveHandlerType<TMessage>(Type resolvedHandlerType)
        {
            return _factory.Create<TMessage>(resolvedHandlerType);
        }
    }

    public class CachingMessageHandlerResolver : MessageHandlerResolver
    {
        private readonly MessageHandlerCache _cache = new MessageHandlerCache();

        public CachingMessageHandlerResolver([NotNull] IMessageHandlerFactory factory, [NotNull] MessageHandlerRegistry registry) : base(factory, registry)
        {
        }

        protected override IMessageHandler<TMessage> ResolveHandlerType<TMessage>(Type resolvedHandlerType)
        {
            if (_cache.Contains(resolvedHandlerType))
                return _cache.Get<TMessage>(resolvedHandlerType);

            var handler = base.ResolveHandlerType<TMessage>(resolvedHandlerType);
            _cache.Add(handler);
            return handler;
        }
    }
}
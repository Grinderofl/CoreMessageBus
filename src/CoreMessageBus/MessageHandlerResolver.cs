using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CoreMessageBus
{
    public class MessageHandlerResolver : IMessageHandlerResolver
    {
        private readonly IMessageHandlerFactory _factory;
        private readonly MessageHandlerRegistry _registry;

        private readonly MessageHandlerCache _cache;

        public MessageHandlerResolver([NotNull]IMessageHandlerFactory factory, [NotNull] MessageHandlerRegistry registry)
        {
            
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
            _factory = factory;
            _registry = registry;
            _cache = new MessageHandlerCache();
        }

        public IEnumerable<IMessageHandler<TMessage>> Resolve<TMessage>()
        {
            var resolvedHandlerTypes = _registry.HandlersFor<TMessage>();
            foreach (var resolverHandlerType in resolvedHandlerTypes)
            {
                if (_cache.Contains(resolverHandlerType))
                {
                    yield return _cache.Get<TMessage>(resolverHandlerType);
                }
                var handler = _factory.Create<TMessage>(resolverHandlerType);
                yield return handler;
            }
        }
    }

    public class MessageHandlerCache
    {
        internal readonly ISet<MessageHandlerCacheItem> CacheItems = new HashSet<MessageHandlerCacheItem>();

        public bool Contains([NotNull] Type resolvedType)
        {
            if (resolvedType == null) throw new ArgumentNullException(nameof(resolvedType));
            return CacheItems.Any(item => item.IsOfType(resolvedType));
        }

        public void Add<TMessage>([NotNull] IMessageHandler<TMessage> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if(CacheItems.Any(item => item.IsOfType(handler.GetType())))
            CacheItems.Add(new MessageHandlerCacheItem<TMessage>(handler));
        }

        public IMessageHandler<TMessage> Get<TMessage>(Type resolverHandlerType)
        {
            return CacheItems.FirstOrDefault(item => item.IsOfType(resolverHandlerType))?.Get<TMessage>();
        }
    }

    public abstract class MessageHandlerCacheItem
    {
        public abstract bool IsOfType(Type handlerType);
        public abstract IMessageHandler<TMessage> Get<TMessage>();
    }

    public class MessageHandlerCacheItem<TMessage> : MessageHandlerCacheItem
    {
        private readonly IMessageHandler<TMessage> _handler;

        public MessageHandlerCacheItem([NotNull] IMessageHandler<TMessage> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handler = handler;
        }

        public override bool IsOfType([NotNull] Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));
            return _handler.GetType() == handlerType;
        }

        public override IMessageHandler<TMessage1> Get<TMessage1>()
        {
            return _handler as IMessageHandler<TMessage1>;
        }
    }
}
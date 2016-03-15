using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CoreMessageBus.Internal
{
    public class MessageHandlerCache
    {
        internal readonly ISet<MessageHandlerCacheItem> CacheItems = new HashSet<MessageHandlerCacheItem>();

        public bool Contains([NotNull] Type resolvedType)
        {
            if (resolvedType == null) throw new ArgumentNullException(nameof(resolvedType));
            return CacheItems.Any(item => item.Contains(resolvedType));
        }

        public void Add<TMessage>([NotNull] IMessageHandler<TMessage> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if(!CacheItems.Any(item => item.Contains(handler.GetType())))
                CacheItems.Add(new MessageHandlerCacheItem<TMessage>(handler));
        }

        public IMessageHandler<TMessage> Get<TMessage>(Type resolverHandlerType)
        {
            return CacheItems.FirstOrDefault(item => item.Contains(resolverHandlerType))?.Get<TMessage>();
        }
    }
}
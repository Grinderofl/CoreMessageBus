using System;
using JetBrains.Annotations;

namespace CoreMessageBus
{
    public class MessageHandlerCacheItem<TMessage> : MessageHandlerCacheItem
    {
        private readonly IMessageHandler<TMessage> _handler;

        public MessageHandlerCacheItem([NotNull] IMessageHandler<TMessage> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handler = handler;
        }

        public override bool Contains([NotNull] Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));
            return _handler.GetType() == handlerType;
        }

        public override IMessageHandler<TMessage1> Get<TMessage1>()
        {
            return _handler as IMessageHandler<TMessage1>;
        }
    }

    public abstract class MessageHandlerCacheItem
    {
        public abstract bool Contains(Type handlerType);
        public abstract IMessageHandler<TMessage> Get<TMessage>();
    }
}
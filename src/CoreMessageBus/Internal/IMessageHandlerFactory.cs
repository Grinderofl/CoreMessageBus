using System;

namespace CoreMessageBus.Internal
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler<TMessage> Create<TMessage>(Type handlerType);
    }
}
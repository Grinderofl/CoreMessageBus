using System;

namespace CoreMessageBus
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler<TMessage> Create<TMessage>(Type handlerType);
    }
}
using System.Collections.Generic;

namespace CoreMessageBus
{
    public interface IMessageHandlerResolver
    {
        IEnumerable<IMessageHandler<TMessage>> Resolve<TMessage>();
    }
}
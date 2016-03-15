using System.Collections.Generic;

namespace CoreMessageBus.Internal
{
    public interface IMessageHandlerResolver
    {
        IEnumerable<IMessageHandler<TMessage>> Resolve<TMessage>();
    }
}
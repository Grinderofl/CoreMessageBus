using System.Collections.Generic;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Internal
{
    public interface IQueueSelector
    {
        Queue GetQueue<TMessage>();
        IEnumerable<Queue> GetQueues();
    }
}
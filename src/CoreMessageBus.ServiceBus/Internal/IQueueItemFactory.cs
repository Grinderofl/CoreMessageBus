using System;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Internal
{
    public interface IQueueItemFactory
    {
        QueueItem Create<TMessage>(TMessage message, DateTime? deferredUntil = null);
    }
}
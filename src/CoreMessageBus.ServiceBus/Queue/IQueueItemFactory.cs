using System;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Queue
{
    public interface IQueueItemFactory
    {
        QueueItem Create<TMessage>(TMessage message, DateTime? deferredUntil = null);
    }
}
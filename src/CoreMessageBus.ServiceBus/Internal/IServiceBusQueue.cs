using System;
using System.Collections.Generic;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Internal
{
    public interface IServiceBusQueue
    {
        QueueItem Peek();
        void Dequeue(QueueItem item);
        void Queue(QueueItem item);
        int GetQueueId(string queueName);
        void Error(MessageBusException messageBusException, Guid id);
        void Success(QueueItem item);
        IEnumerable<Queue> GetQueues();
    }
}
using System;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Queue
{
    public interface IQueueOperations
    {
        QueueItem Peek();
        void Dequeue(QueueItem item);
        void Queue(QueueItem item);
        int GetQueueId(string queueName);
        void Error(MessageBusException messageBusException, Guid id);
        void Success(QueueItem item);
    }
}
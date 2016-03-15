using System;
using System.Threading.Tasks;

namespace CoreMessageBus.ServiceBus
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
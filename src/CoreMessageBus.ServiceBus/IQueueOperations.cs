using System.Threading.Tasks;

namespace CoreMessageBus.ServiceBus
{
    public interface IQueueOperations
    {
        QueueItem Peek();
        Task Dequeue(QueueItem item);
        void Queue(QueueItem item);
    }
}
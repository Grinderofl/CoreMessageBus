using System.Threading.Tasks;

namespace CoreMessageBus.ServiceBus
{
    public interface IQueueOperations
    {
        QueueItem Peek();
        void Dequeue(QueueItem item);
        void Queue(QueueItem item);
    }
}
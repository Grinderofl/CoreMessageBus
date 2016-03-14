using System.Threading.Tasks;

namespace CoreMessageBus.SqlServer
{
    public interface IDatabaseOperations
    {
        QueueItem Peek();
        Task Dequeue(QueueItem item);
    }
}
namespace CoreMessageBus.ServiceBus.Queue
{
    public interface IQueueService
    {
        bool HasQueue();
        void ProcessNextItem();
    }
}
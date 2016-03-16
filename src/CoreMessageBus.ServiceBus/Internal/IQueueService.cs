namespace CoreMessageBus.ServiceBus.Internal
{
    public interface IQueueService
    {
        bool HasQueue();
        void ProcessNextItem();
    }
}
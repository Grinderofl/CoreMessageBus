namespace CoreMessageBus.ServiceBus
{
    public interface IQueueService
    {
        bool HasQueue();
        void ProcessNextItem();
    }
}
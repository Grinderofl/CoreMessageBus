namespace CoreMessageBus.SqlServer
{
    public interface IQueueService
    {
        bool HasQueue();
        void ProcessNextItem();
    }
}
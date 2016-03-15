namespace CoreMessageBus.ServiceBus.Queue
{
    public interface IQueueSelector
    {
        Domain.Queue GetQueue<TMessage>();
    }
}
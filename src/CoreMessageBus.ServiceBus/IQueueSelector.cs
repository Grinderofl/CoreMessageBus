using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus
{
    public interface IQueueSelector
    {
        Queue GetQueue<TMessage>();
    }
}
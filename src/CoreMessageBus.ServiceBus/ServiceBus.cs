using System;
using CoreMessageBus.ServiceBus.Internal;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IServiceBusQueue _serviceBusQueue;
        private readonly IQueueItemFactory _queueItemFactory;

        public ServiceBus(IServiceBusQueue serviceBusQueue, IQueueItemFactory queueItemFactory)
        {
            _serviceBusQueue = serviceBusQueue;
            _queueItemFactory = queueItemFactory;
        }

        public void Send<TMessage>(TMessage message)
        {
            var item = _queueItemFactory.Create(message);
            _serviceBusQueue.Queue(item);
        }

        public void Defer<TMessage>(TMessage message, TimeSpan timespan)
        {
            throw new NotImplementedException();
        }

        public void Defer<TMessage>(TMessage message, DateTime until)
        {
            throw new NotImplementedException();
        }
    }
}
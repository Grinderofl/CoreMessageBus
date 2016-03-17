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
            if (serviceBusQueue == null) throw new ArgumentNullException(nameof(serviceBusQueue));
            if (queueItemFactory == null) throw new ArgumentNullException(nameof(queueItemFactory));
            _serviceBusQueue = serviceBusQueue;
            _queueItemFactory = queueItemFactory;
        }

        public void Send<TMessage>(TMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var item = _queueItemFactory.Create(message);
            _serviceBusQueue.Queue(item);
        }

        public void Defer<TMessage>(TMessage message, TimeSpan timespan)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            throw new NotImplementedException();
        }

        public void Defer<TMessage>(TMessage message, DateTime until)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            throw new NotImplementedException();
        }
    }
}
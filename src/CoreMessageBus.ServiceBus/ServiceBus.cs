using System;
using CoreMessageBus.ServiceBus.Queue;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IQueueOperations _queueOperations;
        private readonly IQueueItemFactory _queueItemFactory;

        public ServiceBus(IQueueOperations queueOperations, IQueueItemFactory queueItemFactory)
        {
            _queueOperations = queueOperations;
            _queueItemFactory = queueItemFactory;
        }

        public void Send<TMessage>(TMessage message)
        {
            var item = _queueItemFactory.Create(message);
            _queueOperations.Queue(item);
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
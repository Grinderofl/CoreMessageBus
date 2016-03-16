using System.Collections.Generic;
using System.Linq;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class QueueSelector : IQueueSelector
    {
        private readonly QueueOptions _queueOptions;
        private readonly IServiceBusQueue _serviceBusQueue;
        private readonly IServiceBusQueue _queue;
        
        public QueueSelector(QueueOptions queueOptions, IServiceBusQueue serviceBusQueue, IServiceBusQueue queue)
        {
            _queueOptions = queueOptions;
            _serviceBusQueue = serviceBusQueue;
            _queue = queue;
        }

        public virtual Queue GetQueue<TMessage>()
        {
            var queueName = _queueOptions.HandlesQueues.FirstOrDefault(x => x.Value.Contains(typeof(TMessage))).Key;
            var queueId = _serviceBusQueue.GetQueueId(queueName);
            return new Domain.Queue() {Id = queueId, Name = queueName};
        }

        public IEnumerable<Queue> GetQueues()
        {
            return _queue.GetQueues();
        }
    }
}
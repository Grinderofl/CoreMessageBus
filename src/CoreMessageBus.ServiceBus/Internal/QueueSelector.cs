using System;
using System.Collections.Generic;
using System.Linq;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class QueueSelector : IQueueSelector
    {
        private readonly QueueOptions _queueOptions;
        private readonly IServiceBusQueue _queue;
        
        public QueueSelector(QueueOptions queueOptions,IServiceBusQueue queue)
        {
            if (queueOptions == null) throw new ArgumentNullException(nameof(queueOptions));
            if (queue == null) throw new ArgumentNullException(nameof(queue));
            _queueOptions = queueOptions;
            _queue = queue;
        }

        public virtual Queue GetQueue<TMessage>()
        {
            var queueName = _queueOptions.HandlesQueues.FirstOrDefault(x => x.Value.Contains(typeof(TMessage))).Key;
            var queueId = _queue.GetQueueId(queueName);
            return new Queue() {Id = queueId, Name = queueName};
        }

        public IEnumerable<Queue> GetQueues()
        {
            return _queue.GetQueues();
        }
    }
}
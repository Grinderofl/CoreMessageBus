using System.Linq;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus
{
    public class QueueSelector : IQueueSelector
    {
        private readonly QueueOptions _queueOptions;
        private readonly IQueueOperations _queueOperations;

        public QueueSelector(QueueOptions queueOptions, IQueueOperations queueOperations)
        {
            _queueOptions = queueOptions;
            _queueOperations = queueOperations;
        }

        public virtual Queue GetQueue<TMessage>()
        {
            var queueName = _queueOptions.HandlesQueues.FirstOrDefault(x => x.Value.Contains(typeof(TMessage))).Key;
            var queueId = _queueOperations.GetQueueId(queueName);
            return new Queue() {Id = queueId, Name = queueName};
        }
    }
}
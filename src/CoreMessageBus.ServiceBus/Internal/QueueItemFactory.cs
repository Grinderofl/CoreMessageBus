using System;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class QueueItemFactory : IQueueItemFactory
    {
        private readonly IDataSerializer _dataSerializer;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IIdGenerator _generator;
        private readonly IQueueSelector _selector;

        public QueueItemFactory(IDataSerializer dataSerializer, IDateTimeProvider dateTimeProvider, IIdGenerator generator, IQueueSelector selector)
        {
            if (dataSerializer == null) throw new ArgumentNullException(nameof(dataSerializer));
            if (dateTimeProvider == null) throw new ArgumentNullException(nameof(dateTimeProvider));
            if (generator == null) throw new ArgumentNullException(nameof(generator));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            _dataSerializer = dataSerializer;
            _dateTimeProvider = dateTimeProvider;
            _generator = generator;
            _selector = selector;
        }

        public virtual QueueItem Create<TMessage>(TMessage message, DateTime? deferredUntil = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var queueItem = new QueueItem();
            queueItem.Type = typeof (TMessage);
            queueItem.ContentType = _dataSerializer.ContentType;
            queueItem.Created = _dateTimeProvider.Now;
            queueItem.Encoding = _dataSerializer.Encoding;
            queueItem.Id = _generator.Create();
            queueItem.MessageId = _generator.Create();
            queueItem.Queue = _selector.GetQueue<TMessage>();
            queueItem.Deferred = deferredUntil;
            queueItem.Data = _dataSerializer.Serialize(message);
            return queueItem;
        }
    }
}
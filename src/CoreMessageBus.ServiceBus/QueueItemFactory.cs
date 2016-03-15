using System;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus
{
    public class QueueItemFactory
    {
        private readonly IDataSerializer _dataSerializer;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IIdGenerator _generator;
        private readonly IQueueSelector _selector;

        public QueueItemFactory(IDataSerializer dataSerializer, IDateTimeProvider dateTimeProvider, IIdGenerator generator, IQueueSelector selector)
        {
            _dataSerializer = dataSerializer;
            _dateTimeProvider = dateTimeProvider;
            _generator = generator;
            _selector = selector;
        }

        public QueueItem Create<TMessage>(TMessage message, DateTime? deferredUntil = null)
        {
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
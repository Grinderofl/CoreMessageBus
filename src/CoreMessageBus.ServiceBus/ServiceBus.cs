using System;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IQueueOperations _queueOperations;
        private readonly QueueItemFactory _queueItemFactory;

        public ServiceBus(IQueueOperations queueOperations, QueueItemFactory queueItemFactory)
        {
            _queueOperations = queueOperations;
            _queueItemFactory = queueItemFactory;
        }

        public void Send<TMessage>(TMessage message)
        {
            //var queue = _queueSelector.GetQueue<TMessage>();
            
            //var queueItem = new QueueItem()
            //{
            //    Type = typeof (TMessage),
            //    ContentType = "application/json",
            //    Created = DateTime.Now,
            //    Data = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
            //    {
            //        PreserveReferencesHandling = PreserveReferencesHandling.Objects
            //    }),
            //    Encoding = Encoding.UTF8,
            //    Id = Guid.NewGuid(),
            //    MessageId = Guid.NewGuid(),
            //    Queue = queue
            //};
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
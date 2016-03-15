using System;
using System.Text;
using Newtonsoft.Json;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private IQueueOperations _queueOperations;

        public ServiceBus(IQueueOperations queueOperations)
        {
            _queueOperations = queueOperations;
        }

        public void Send<TMessage>(TMessage message)
        {
            var queueItem = new QueueItem()
            {
                Type = typeof (TMessage),
                ContentType = "application/json",
                Created = DateTime.Now,
                Data = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                }),
                Encoding = Encoding.UTF8,
                Id = Guid.NewGuid(),
                MessageId = Guid.NewGuid()
            };
            _queueOperations.Queue(queueItem);
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
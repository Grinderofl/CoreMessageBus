using System;
using System.Linq;
using System.Text;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Domain;
using Newtonsoft.Json;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IQueueOperations _queueOperations;
        private readonly QueueOptions _queueOptions;

        public ServiceBus(IQueueOperations queueOperations, QueueOptions queueOptions)
        {
            _queueOperations = queueOperations;
            _queueOptions = queueOptions;
        }

        public void Send<TMessage>(TMessage message)
        {
            var queueName =
                _queueOptions.HandlesQueues.FirstOrDefault(x => x.Value.Contains(typeof (TMessage))).Key;

            var queueId = _queueOperations.GetQueueId(queueName);

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
                MessageId = Guid.NewGuid(),
                Queue = new Queue()
                {
                    Name = queueName,
                    Id = queueId
                }
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
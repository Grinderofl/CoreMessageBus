using System;
using System.Collections.Generic;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBusOptions
    {
        public bool SendOnlyServiceBus { get; protected set; }

        public ServiceBusOptions SendOnly(bool sendOnly = true)
        {
            SendOnlyServiceBus = sendOnly;
            return this;
        }

        public Type QueueOperations { get; protected set; }

        public ServiceBusOptions Operations<TOperations>() where TOperations : IQueueOperations
        {
            QueueOperations = typeof(TOperations);
            return this;
        }

        public ServiceBusOptions Handles(string queueName, IEnumerable<Type> messageTypes)
        {
            QueueOptions.Queue(queueName, messageTypes);
            return this;
        }

        public QueueOptions QueueOptions { get; set; } = new QueueOptions();
    }
}
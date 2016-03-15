using System;
using System.Collections.Generic;
using CoreMessageBus.ServiceBus.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMessageBus.ServiceBus.Configuration
{
    public class ServiceBusOptions : IServiceBusInfrastructure
    {
        private readonly IServiceCollection _services;

        public ServiceBusOptions(IServiceCollection services)
        {
            _services = services;
        }

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

        IServiceCollection IServiceBusInfrastructure.Services => _services;
    }
}
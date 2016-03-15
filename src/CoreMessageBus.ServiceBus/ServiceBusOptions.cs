using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMessageBus.ServiceBus
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

    public static class ServiceBusInfrastructureExtensions
    {
        public static IServiceBusInfrastructure GetInfrastructure<T>(this T infrastructure) where T : IServiceBusInfrastructure
        {
            return (IServiceBusInfrastructure) infrastructure;
        }
    }

    public interface IServiceBusInfrastructure
    {
        IServiceCollection Services { get; }
    }
}
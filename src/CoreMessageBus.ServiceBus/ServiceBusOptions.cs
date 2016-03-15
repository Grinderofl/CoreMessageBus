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

        public IQueueOperations QueueOperations { get; protected set; }

        public ServiceBusOptions Operations<TOperations>(TOperations instance) where TOperations : IQueueOperations
        {
            QueueOperations = instance;
            return this;
        }

    }
}
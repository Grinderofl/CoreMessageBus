using System.Reflection;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class QueueService : IQueueService
    {
        private readonly IServiceBusQueue _serviceBusQueue;
        protected readonly IMessageBus MessageBus;

        public QueueService(IServiceBusQueue serviceBusQueue, IMessageBus messageBus)
        {
            _serviceBusQueue = serviceBusQueue;
            MessageBus = messageBus;
        }

        public bool HasQueue()
        {
            return _serviceBusQueue.Peek() != null;
        }
        
        public void ProcessNextItem()
        {
            var item = _serviceBusQueue.Peek();
            if (item == null)
                return;
            _serviceBusQueue.Dequeue(item);
            var sendMethod = typeof(IMessageBus).GetTypeInfo().GetDeclaredMethod("Send");
            var sendGenericMethod = sendMethod.MakeGenericMethod(item.Type);
            var message = item.Data;
            try
            {
                sendGenericMethod.Invoke(MessageBus, new[] {message});
                _serviceBusQueue.Success(item);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is MessageBusException)
                {
                    _serviceBusQueue.Error(ex.InnerException as MessageBusException, item.Id);
                }
                else
                {
                    throw;
                }

            }
        }
    }


}
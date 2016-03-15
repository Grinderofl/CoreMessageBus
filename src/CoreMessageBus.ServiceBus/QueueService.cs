using System;
using System.Reflection;

namespace CoreMessageBus.ServiceBus
{
    public class QueueService : IQueueService
    {
        private readonly IQueueOperations _queueOperations;
        protected readonly IMessageBus MessageBus;

        public QueueService(IQueueOperations queueOperations, IMessageBus messageBus)
        {
            _queueOperations = queueOperations;
            MessageBus = messageBus;
        }

        public bool HasQueue()
        {
            return _queueOperations.Peek() != null;
        }
        
        public void ProcessNextItem()
        {
            var item = _queueOperations.Peek();
            if (item == null)
                return;
            _queueOperations.Dequeue(item);
            var sendMethod = typeof(IMessageBus).GetTypeInfo().GetDeclaredMethod("Send");
            var sendGenericMethod = sendMethod.MakeGenericMethod(item.Type);
            var message = item.Data;
            try
            {
                sendGenericMethod.Invoke(MessageBus, new[] {message});
                _queueOperations.Success(item);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is MessageBusException)
                {
                    _queueOperations.Error(ex.InnerException as MessageBusException, item.Id);
                }
                else
                {
                    throw;
                }

            }
        }
    }


}
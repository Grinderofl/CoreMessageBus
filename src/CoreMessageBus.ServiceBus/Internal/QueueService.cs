using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class QueueService : IQueueService
    {
        private readonly IServiceBusQueue _serviceBusQueue;
        protected readonly IMessageBus MessageBus;
        private QueueProcessorCache _cache;

        public QueueService(IServiceBusQueue serviceBusQueue, IMessageBus messageBus)
        {
            _serviceBusQueue = serviceBusQueue;
            MessageBus = messageBus;
            _cache = new QueueProcessorCache();
        }

        public bool HasQueue()
        {
            return _serviceBusQueue.Peek() != null;
        }
        
        public void ProcessNextItem()
        {
            var item = _serviceBusQueue.Peek();
            if (item == null || _cache.Contains(item))
                return;
            _cache.Add(item);
            _serviceBusQueue.Dequeue(item);
            var sendMethod = typeof(IMessageBus).GetTypeInfo().GetDeclaredMethod("Send");
            var sendGenericMethod = sendMethod.MakeGenericMethod(item.Type);
            var message = item.Data;
            try
            {
                sendGenericMethod.Invoke(MessageBus, new[] {message});
                _serviceBusQueue.Success(item);
                _cache.Remove(item);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is MessageBusException)
                {
                    _serviceBusQueue.Error(ex.InnerException as MessageBusException, item.Id);
                    _cache.Remove(item);
                }
                else
                {
                    throw;
                }

            }
        }
    }


}
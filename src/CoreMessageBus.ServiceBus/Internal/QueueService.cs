using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class QueueService : IQueueService
    {
        private readonly IServiceBusQueue _serviceBusQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceBusUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly QueueProcessorCache _cache;

        public QueueService(IServiceBusQueue serviceBusQueue, IServiceScopeFactory serviceScopeFactory, IServiceBusUnitOfWork unitOfWork, ILogger logger)
        {
            _serviceBusQueue = serviceBusQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _unitOfWork = unitOfWork;
            _logger = logger;
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

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var messageBus = scope.ServiceProvider.GetService<IMessageBus>();

                try
                {
                    _unitOfWork.Begin();
                    sendGenericMethod.Invoke(messageBus, new[] { message });
                    _unitOfWork.Commit();
                    _serviceBusQueue.Success(item);
                    _cache.Remove(item);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is MessageBusException)
                    {
                        _logger.LogError("");
                        _unitOfWork.Rollback();
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


}
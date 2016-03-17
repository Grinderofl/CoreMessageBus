using System.Collections.Generic;
using System.Threading;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Internal;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBusClient : IServiceBusClient
    {
        private readonly IList<Thread> _threads = new List<Thread>();

        private bool _started;

        private readonly IQueueService _queueService;
        private readonly QueueOptions _options;

        public ServiceBusClient(IQueueService queueService, QueueOptions options)
        {
            _queueService = queueService;
            _options = options;
        }

        public void Start()
        {
            _started = true;
            for (var i = 0; i < _options.Workers; i++)
            {
                var thread = new Thread(() =>
                {
                    while (_started)
                    {
                        if (!_queueService.HasQueue())
                        {
                            Thread.Sleep(_options.SleepTime);
                            continue;
                        }
                        _queueService.ProcessNextItem();
                    }
                });
                thread.Start();
                _threads.Add(thread);
            }
            
        }
        

        public void Stop()
        {
            _started = false;
            _threads.Clear();
        }
    }
}

using System.Threading;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBusClient : IServiceBusClient
    {
        private Thread _thread;
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
            _thread = new Thread(() =>
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
            _thread.Start();
        }
        

        public void Stop()
        {
            _started = false;
        }
    }
}

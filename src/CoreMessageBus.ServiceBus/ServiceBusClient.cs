using System.Threading;

namespace CoreMessageBus.ServiceBus
{
    public class ServiceBusClient : IServiceBusClient
    {
        private Thread _thread;
        private bool _started;

        private readonly IQueueService _queueService;

        public ServiceBusClient(IQueueService queueService)
        {
            _queueService = queueService;
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
                        Thread.Sleep(500);
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

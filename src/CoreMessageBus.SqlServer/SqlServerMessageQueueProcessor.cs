using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CoreMessageBus.SqlServer
{
    public class SqlServerMessageQueueProcessor : IMessageQueueProcessor
    {
        private readonly IDatabaseOperations _databaseOperations;
        private readonly IMessageHandlerResolver _resolver;
        private Thread _thread;
        private bool _started;

        private readonly IQueueService _queueService;

        public SqlServerMessageQueueProcessor(IDatabaseOperations databaseOperations, IMessageHandlerResolver resolver, IQueueService queueService)
        {
            _databaseOperations = databaseOperations;
            _resolver = resolver;
            _queueService = queueService;
        }

        public void Start()
        {
            _started = true;
            new Thread(() =>
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
            }).Start();
        }
        

        public void Stop()
        {
            _started = false;
        }
    }
}

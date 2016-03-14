using System.Reflection;

namespace CoreMessageBus.SqlServer
{
    public class SqlServerQueueService : IQueueService
    {
        private readonly IDatabaseOperations _databaseOperations;
        private readonly IMessageBus _messageBus;

        public SqlServerQueueService(IDatabaseOperations databaseOperations, IMessageBus messageBus)
        {
            _databaseOperations = databaseOperations;
            _messageBus = messageBus;
        }

        public bool HasQueue()
        {
            return (_databaseOperations.Peek() != null);
        }

        public void ProcessNextItem()
        {
            var item = _databaseOperations.Peek();
            _databaseOperations.Dequeue(item);
            var sendMethod = typeof (IMessageBus).GetTypeInfo().GetDeclaredMethod("Send");
            var sendGenericMethod = sendMethod.MakeGenericMethod(item.Type);
            var message = item.Data;
            sendGenericMethod.Invoke(_messageBus, new[] {message});
        }
    }
}
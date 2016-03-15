using CoreMessageBus.ServiceBus;

namespace CoreMessageBus.SqlServer
{
    public class SqlServerQueueOperationOptions
    {
        public string SchemaName { get; set; } = "dbo";
        public string QueuesTableName { get; set; } = "SqlServerQueues";
        public string QueueTableName { get; set; } = "SqlServerQueue";
    }
}
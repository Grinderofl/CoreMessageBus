namespace CoreMessageBus.ServiceBus.SqlServer.Configuration
{
    public class SqlServerQueueOperationOptions
    {
        public string SchemaName { get; set; } = "dbo";
        public string QueuesTableName { get; set; } = "SqlServerQueues";
        public string QueueTableName { get; set; } = "SqlServerQueue";
    }
}
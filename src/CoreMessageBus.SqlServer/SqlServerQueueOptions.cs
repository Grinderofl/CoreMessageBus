using CoreMessageBus.ServiceBus;

namespace CoreMessageBus.SqlServer
{
    public class SqlServerQueueOptions : QueueOptions
    {
        public SqlServerQueueOptions(QueueOptions options)
        {
            this.HandlesQueues = options.HandlesQueues;
        }
        public string ConnectionString { get; set; }
        public string SchemaName { get; set; } = "dbo";
        public string QueuesTableName { get; set; } = "SqlServerQueues";
        public string QueueTableName { get; set; } = "SqlServerQueue";
    }
}
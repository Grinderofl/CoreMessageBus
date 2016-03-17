using System;

namespace CoreMessageBus.ServiceBus.SqlServer.Configuration
{
    public class SqlServerQueueOperationOptions
    {
        public string SchemaName { get; set; } = "dbo";
        public string QueuesTableName { get; set; } = "SqlServerQueues";
        public string QueueItemsTableName { get; set; } = "SqlServerQueueItems";
        public string ConnectionStringValue { get; set; }

        private SqlServerQueueOperationOptions SetOption(Action<SqlServerQueueOperationOptions> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            action(this);
            return this;
        }

        public SqlServerQueueOperationOptions Schema(string schemaName) 
            => SetOption(x => x.SchemaName = schemaName);

        public SqlServerQueueOperationOptions QueuesTable(string queuesTableName)
            => SetOption(x => x.QueuesTableName = queuesTableName);

        public SqlServerQueueOperationOptions QueueItemsTable(string queueItemsTableName)
            => SetOption(x => x.QueueItemsTableName = queueItemsTableName);

        public SqlServerQueueOperationOptions ConnectionString(string connectionStringValue)
            => SetOption(x => x.ConnectionStringValue = connectionStringValue);
    }
}
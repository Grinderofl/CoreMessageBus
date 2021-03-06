using System;
using CoreMessageBus.ServiceBus.SqlServer.Configuration;

namespace CoreMessageBus.ServiceBus.SqlServer.Internal
{
    public class SqlQueries
    {
        private string DelimitIdentifier(string identifier)
        {
            return "[" + identifier.Replace("]", "]]") + "]";
        }

        private string _peekFormat = 
            "SELECT TOP (1) Id, MessageId, ContentType, Encoding, Type, Data, Created, Deferred, Status, {0}.QueueId AS QueueId, {1}.Name AS QueueName " +
            "FROM {0} " +
            "LEFT JOIN {1} ON {1}.QueueId = {0}.QueueId " +
            "WHERE (Deferred IS NULL OR Deferred < getdate()) " +
            "AND Status = 'Queued' " +
            "AND {1}.Name IN ({{QueueName}}) " +
            "ORDER BY Deferred ASC, Created DESC ";
        private string _deQueue = "UPDATE {0} SET Status = 'Dequeued' WHERE Id = @Id";
        private string _error = "UPDATE {0} SET Status = 'Error', Error = @Error  WHERE Id = @Id";
        private string _processed = "UPDATE {0} SET Status = 'Processed' WHERE Id = @Id";

        private string _queue = "INSERT INTO {0} " +
                                "(Id, MessageId, ContentType, Encoding, Type, Data, Created, Deferred, Status, QueueId)" +
                                "VALUES (@Id, @MessageId, @ContentType, @Encoding, @Type, @Data, @Created, @Deferred, 'Queued', @QueueId)";

        private string _queueId = "SELECT TOP (1) QueueId FROM {0} WHERE Name = @Name";

        private string _queues = "SELECT QueueId, Name FROM {0} ORDER BY Name DESC";

        public SqlQueries(SqlServerQueueOperationOptions operationOptions)
        {
            if (operationOptions == null) throw new ArgumentNullException(nameof(operationOptions));
            var queueTableNameWithSchema = $"{DelimitIdentifier(operationOptions.SchemaName)}.{DelimitIdentifier(operationOptions.QueueItemsTableName)}";
            var queuesTableNameWithSchema =
                $"{DelimitIdentifier(operationOptions.SchemaName)}.{DelimitIdentifier(operationOptions.QueuesTableName)}";

            PeekQueue = string.Format(_peekFormat, queueTableNameWithSchema, queuesTableNameWithSchema);
            DeQueue = string.Format(_deQueue, queueTableNameWithSchema);
            Error = string.Format(_error, queueTableNameWithSchema);
            Queue = string.Format(_queue, queueTableNameWithSchema);
            QueueId = string.Format(_queueId, queuesTableNameWithSchema);
            Processed = string.Format(_processed, queueTableNameWithSchema);
            Queues = string.Format(_queues, queuesTableNameWithSchema);
        }

        public string Processed { get; }
        public string PeekQueue { get; }
        public string DeQueue { get; }
        public string Error { get; }
        public string Queue { get; }
        public string QueueId { get; }
        public string Queues { get; }
    }

    
}
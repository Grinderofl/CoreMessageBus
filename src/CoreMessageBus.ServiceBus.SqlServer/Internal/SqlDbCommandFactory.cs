using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CoreMessageBus.ServiceBus.Domain;
using CoreMessageBus.ServiceBus.SqlServer.Configuration;
using CoreMessageBus.ServiceBus.SqlServer.Extensions;
using System.Linq;

namespace CoreMessageBus.ServiceBus.SqlServer.Internal
{
    public class SqlDbCommandFactory : IDbCommandFactory
    {
        private readonly SqlServerQueueOperationOptions _operationOptions;
        private readonly SqlQueries _queries;

        public SqlDbCommandFactory(SqlServerQueueOperationOptions operationOptions)
        {
            _operationOptions = operationOptions;
            _queries = new SqlQueries(operationOptions);
        }

        public SqlCommand CreatePeekCommand(IEnumerable<Queue> queues)
        {
            if (queues == null) throw new ArgumentNullException(nameof(queues));
            var command = new SqlCommand(_queries.PeekQueue);
            command.AddArrayParameters(queues.Select(x => x.Name), "QueueName");
            return command;
        }

        public SqlCommand CreateDequeueCommand(QueueItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var command = new SqlCommand(_queries.DeQueue);
            command.Parameters.AddWithValue("@Id", item.Id);
            return command;
        }

        public SqlCommand CreateQueueCommand(QueueItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var command = new SqlCommand(_queries.Queue);
            command.Parameters
                .AddParameter(Columns.Names.Id, item.Id)
                .AddParameter(Columns.Names.MessageId, item.MessageId)
                .AddParameter(Columns.Names.Type, item.Type.AssemblyQualifiedName)
                .AddParameter(Columns.Names.ContentType, item.ContentType)
                .AddParameter(Columns.Names.Created, item.Created)
                .AddParameter(Columns.Names.Deferred, item.Deferred)
                .AddParameter(Columns.Names.Data, item.Data)
                .AddParameter(Columns.Names.QueueId, item.Queue.Id)
                .AddParameter(Columns.Names.Encoding, item.Encoding.WebName);
            return command;
        }

        public SqlCommand CreateQueueIdCommand(string queueName)
        {
            if (queueName == null) throw new ArgumentNullException(nameof(queueName));
            var command = new SqlCommand(_queries.QueueId);
            command.Parameters.AddParameter(Columns.Names.Name, queueName);
            return command;
        }

        public SqlCommand CreateErrorCommand(MessageBusException messageBusException, Guid queueItemId)
        {
            if (messageBusException == null) throw new ArgumentNullException(nameof(messageBusException));
            var command = new SqlCommand(_queries.Error);
            command.Parameters.AddWithValue("@Id", queueItemId);
            command.Parameters.AddWithValue("@Error",
                $"{messageBusException.Message}{Environment.NewLine}{messageBusException.InnerException}{Environment.NewLine}{messageBusException.StackTrace}");
            return command;
        }

        public SqlCommand CreateSuccessCommand(QueueItem queueItem)
        {
            if (queueItem == null) throw new ArgumentNullException(nameof(queueItem));
            var command = new SqlCommand(_queries.Processed);
            command.Parameters.AddWithValue("@Id", queueItem.Id);
            return command;
        }

        public SqlCommand CreateQueuesCommand()
        {
            var command = new SqlCommand(_queries.Queues);
            return command;
        }
    }
}
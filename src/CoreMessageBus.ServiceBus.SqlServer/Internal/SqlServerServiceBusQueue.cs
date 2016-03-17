using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CoreMessageBus.ServiceBus.Domain;
using CoreMessageBus.ServiceBus.Internal;
using CoreMessageBus.ServiceBus.SqlServer.Extensions;

namespace CoreMessageBus.ServiceBus.SqlServer.Internal
{
    public class SqlServerServiceBusQueue : IServiceBusQueue
    {
        
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly SqlQueueItemFactory _queueItemFactory;
        private readonly IDbCommandFactory _commandFactory;

        public SqlServerServiceBusQueue(ISqlConnectionFactory connectionFactory, SqlQueueItemFactory queueItemFactory, IDbCommandFactory commandFactory)
        {
            if (connectionFactory == null) throw new ArgumentNullException(nameof(connectionFactory));
            if (queueItemFactory == null) throw new ArgumentNullException(nameof(queueItemFactory));
            if (commandFactory == null) throw new ArgumentNullException(nameof(commandFactory));
            _connectionFactory = connectionFactory;
            _queueItemFactory = queueItemFactory;
            _commandFactory = commandFactory;
        }

        private void Execute(SqlCommand command)
        {
            PerformOnConnection(connection =>
            {
                command.Connection = connection;
                command.ExecuteNonQuery();
            });
        }

        private void PerformOnConnection(Action<SqlConnection> connectionAction)
        {
            using (var connection = _connectionFactory.Create())
            {
                connection.Open();
                connectionAction(connection);
            }
        }

        private void ExecuteReader(SqlCommand command, Action<SqlDataReader> action)
        {
            PerformOnConnection(connection =>
            {
                command.Connection = connection;
                var reader = command.ExecuteReader();
                action(reader);
            });
        }

        public QueueItem Peek()
        {
            var queues = GetQueues();
            var command = _commandFactory.CreatePeekCommand(queues);
            QueueItem queueItem = null;
            ExecuteReader(command, reader => queueItem = _queueItemFactory.Create(reader));
            return queueItem;
        }

        public void Dequeue(QueueItem item)
        {
            var command = _commandFactory.CreateDequeueCommand(item);
            Execute(command);
        }

        public void Queue(QueueItem item)
        {
            var command = _commandFactory.CreateQueueCommand(item);
            Execute(command);
        }

        public int GetQueueId(string queueName)
        {
            var command = _commandFactory.CreateQueueIdCommand(queueName);
            int queueId = -1;
            ExecuteReader(command, reader =>
            {
                if (reader.Read())
                {
                    if (reader.IsDBNull(0))
                        throw new QueueNotFoundException($"Queue {queueName} could not be found.");
                    queueId = reader.GetValue<int>(0);
                }
            });
            if(queueId > -1)
                return queueId;
            throw new QueueNotFoundException($"Queue {queueName} could not be found.");
        }

        public void Error(MessageBusException messageBusException, Guid id)
        {
            var command = _commandFactory.CreateErrorCommand(messageBusException, id);
            Execute(command);
        }

        public void Success(QueueItem item)
        {
            var command = _commandFactory.CreateSuccessCommand(item);
            Execute(command);
        }

        public IEnumerable<Queue> GetQueues()
        {
            var command = _commandFactory.CreateQueuesCommand();
            var queues = new List<Queue>();

            ExecuteReader(command, reader =>
            {
                while (reader.Read())
                {
                    var id = reader.GetValue<int>(Columns.Indexes.IdIndex);
                    var name = reader.GetValue<string>(Columns.Indexes.NameIndex);
                    queues.Add(new Queue(id, name));
                }
            });
            return queues;
        }
    }
}
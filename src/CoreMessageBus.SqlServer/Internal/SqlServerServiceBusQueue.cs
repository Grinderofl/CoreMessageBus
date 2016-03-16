using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CoreMessageBus.ServiceBus.Domain;
using CoreMessageBus.ServiceBus.Extensions;
using CoreMessageBus.ServiceBus.Internal;
using Newtonsoft.Json;

namespace CoreMessageBus.SqlServer.Internal
{
    public class SqlServerServiceBusQueue : IServiceBusQueue
    {
        
        private readonly IConnectionStringSource _connectionStringSource;
        private readonly SqlQueueItemFactory _queueItemFactory;
        private readonly IDbCommandFactory _factory;
        public SqlServerServiceBusQueue(IConnectionStringSource connectionStringSource, SqlQueueItemFactory queueItemFactory, IDbCommandFactory factory)
        {
            _connectionStringSource = connectionStringSource;
            _queueItemFactory = queueItemFactory;
            _factory = factory;
        }

        private string ConnectionString => _connectionStringSource.GetConnectionString();

        private void Execute(SqlCommand command)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void ExecuteReader(SqlCommand command, Action<SqlDataReader> action)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                command.Connection = connection;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                action(reader);
            }
        }

        public QueueItem Peek()
        {
            var queues = GetQueues();
            var command = _factory.CreatePeekCommand(queues);
            QueueItem queueItem = null;
            ExecuteReader(command, reader => queueItem = _queueItemFactory.Create(reader));
            return queueItem;
        }

        public void Dequeue(QueueItem item)
        {
            var command = _factory.CreateDequeueCommand(item);
            Execute(command);
        }

        public void Queue(QueueItem item)
        {
            var command = _factory.CreateQueueCommand(item);
            Execute(command);
        }

        public int GetQueueId(string queueName)
        {
            var command = _factory.CreateQueueIdCommand(queueName);
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
            var command = _factory.CreateErrorCommand(messageBusException, id);
            Execute(command);
        }

        public void Success(QueueItem item)
        {
            var command = _factory.CreateSuccessCommand(item);
            Execute(command);
        }

        public IEnumerable<Queue> GetQueues()
        {
            var command = _factory.CreateQueuesCommand();
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
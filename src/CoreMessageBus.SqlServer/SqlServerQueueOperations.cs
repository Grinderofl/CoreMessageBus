using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CoreMessageBus.ServiceBus;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Domain;
using CoreMessageBus.SqlServer.Extensions;
using Newtonsoft.Json;

namespace CoreMessageBus.SqlServer
{
    public class SqlServerQueueOperations : IQueueOperations
    {
        private readonly QueueOptions _queueOptions;
        private readonly IConnectionStringSource _connectionStringSource;
        private readonly SqlQueries _queries;

        public SqlServerQueueOperations(SqlServerQueueOperationOptions operationOptions, QueueOptions queueOptions, IConnectionStringSource connectionStringSource)
        {
            _queueOptions = queueOptions;
            _connectionStringSource = connectionStringSource;
            _queries = new SqlQueries(operationOptions);
        }

        private string ConnectionString => _connectionStringSource.GetConnectionString();

        public QueueItem Peek()
        {
            var settings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(_queries.PeekQueue, connection);
                command.AddArrayParameters(_queueOptions.HandlesQueues.Select(x => x.Key), "QueueName");
                connection.Open();
                var reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    var item = new QueueItem();
                    item.Queue = new Queue();

                    item.Id = GetValue<Guid>(reader, Columns.Indexes.IdIndex);
                    item.MessageId = GetValue<Guid>(reader, Columns.Indexes.MessageIdIndex);

                    item.Created = GetValue<DateTime>(reader, Columns.Indexes.CreatedIndex);
                    item.Deferred = GetValue<DateTime?>(reader, Columns.Indexes.DeferredIndex);

                    item.ContentType = GetValue<string>(reader, Columns.Indexes.ContentTypeIndex);


                    var typeString = GetValue<string>(reader, Columns.Indexes.TypeIndex);
                    item.Type = Type.GetType(typeString, false);
                    
                    var dataAsString = GetValue<string>(reader, Columns.Indexes.DataIndex);
                    item.Data = JsonConvert.DeserializeObject(dataAsString, item.Type, settings);
                   
                    var encodingAsString = GetValue<string>(reader, Columns.Indexes.EncodingIndex);
                    item.Encoding = Encoding.GetEncoding(encodingAsString);

                    item.Queue.Id = GetValue<int>(reader, Columns.Indexes.QueueIdIndex);
                    item.Queue.Name = GetValue<string>(reader, Columns.Indexes.QueueNameIndex);
                    
                    return item;
                }
                return null;
            }
        }

        private T GetValue<T>(SqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
                return reader.GetFieldValue<T>(index);
            return default(T);
        }

        public void Dequeue(QueueItem item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(_queries.DeQueue, connection);
                command.Parameters.AddWithValue("@Id", item.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Queue(QueueItem item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var insertCommand = new SqlCommand(_queries.Queue, connection);
                insertCommand.Parameters
                    .AddParameter(Columns.Names.Id, item.Id)
                    .AddParameter(Columns.Names.MessageId, item.MessageId)
                    .AddParameter(Columns.Names.Type, item.Type.AssemblyQualifiedName)
                    .AddParameter(Columns.Names.ContentType, item.ContentType)
                    .AddParameter(Columns.Names.Created, item.Created)
                    .AddParameter(Columns.Names.Deferred, item.Deferred)
                    .AddParameter(Columns.Names.Data, item.Data)
                    .AddParameter(Columns.Names.QueueId, item.Queue.Id)
                    .AddParameter(Columns.Names.Encoding, item.Encoding.WebName);
                
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
        }

        public int GetQueueId(string queueName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(_queries.QueueId, connection);
                command.Parameters.AddParameter(Columns.Names.Name, queueName);
                connection.Open();
                var reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    if(reader.IsDBNull(0))
                        throw new QueueNotFoundException($"Queue {queueName} could not be found.");
                    return GetValue<int>(reader, 0);
                }
            }
            throw new QueueNotFoundException($"Queue {queueName} could not be found.");
        }

        public void Error(MessageBusException messageBusException, Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(_queries.Error, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Error",
                    $"{messageBusException.Message}{Environment.NewLine}{messageBusException.InnerException}{Environment.NewLine}{messageBusException.StackTrace}");
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Success(QueueItem item)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(_queries.Processed, connection);
                command.Parameters.AddWithValue("@Id", item.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
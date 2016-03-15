using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CoreMessageBus.ServiceBus;
using Newtonsoft.Json;

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

    public static class ServiceBusOptionsExtensions
    {
        public static ServiceBusOptions UseSqlServer(this ServiceBusOptions options, string connectionString)
        {
            options.Operations<SqlServerQueueOperations>();
            var queueOptions = new SqlServerQueueOptions(options.QueueOptions) {ConnectionString = connectionString};
            options.QueueOptions = queueOptions;
            return options;
        }
    }

    public class SqlServerQueueOperations : IQueueOperations
    {
        private readonly SqlServerQueueOptions _options;
        private readonly string _connectionString;
        private readonly SqlQueries _queries;

        public SqlServerQueueOperations(SqlServerQueueOptions options)
        {
            _options = options;
            _queries = new SqlQueries(options);
            _connectionString = options.ConnectionString;
        }

        public QueueItem Peek()
        {
            var settings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            Debug.WriteLine("Creating connection...");
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(_queries.PeekQueue, connection);
                command.AddArrayParameters(_options.HandlesQueues.Select(x => x.Key), "QueueName");
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
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(_queries.DeQueue, connection);
                command.Parameters.AddWithValue("@Id", item.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Queue(QueueItem item)
        {
            using (var connection = new SqlConnection(_connectionString))
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
            using (var connection = new SqlConnection(_connectionString))
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
            using (var connection = new SqlConnection(_connectionString))
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
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(_queries.Processed, connection);
                command.Parameters.AddWithValue("@Id", item.Id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public class QueueNotFoundException : Exception
    {
        public QueueNotFoundException()
        {
        }

        public QueueNotFoundException(string message) : base(message)
        {
        }

        public QueueNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
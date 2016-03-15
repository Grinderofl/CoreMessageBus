using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using CoreMessageBus.ServiceBus;
using Newtonsoft.Json;

namespace CoreMessageBus.SqlServer
{
    public class SqlServerQueueOperations : IQueueOperations
    {
        private readonly string _connectionString;
        private readonly SqlQueries _queries;

        public SqlServerQueueOperations(string connectionString, string schemaName, string tableName)
        {
            _queries = new SqlQueries(schemaName, tableName);
            _connectionString = connectionString;
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
                connection.Open();
                var reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    var item = new QueueItem();

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

        public async Task Dequeue(QueueItem item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(_queries.DeQueue, connection);
                command.Parameters.AddWithValue("@Id", item.Id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
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
                    //.AddParameter(Columns.Names.Status, "Queued")
                    .AddParameter(Columns.Names.Encoding, item.Encoding.WebName);
                connection.Open();
                insertCommand.ExecuteNonQuery();
            }
        }
    }
}
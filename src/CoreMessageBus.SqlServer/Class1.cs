using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreMessageBus.SqlServer
{
    public class SqlServerMessageQueueProcessor : IMessageQueueProcessor
    {
        private readonly IDatabaseOperations _databaseOperations;
        private readonly IMessageHandlerResolver _resolver;
        private Thread _thread;
        private bool _started;

        public SqlServerMessageQueueProcessor(IDatabaseOperations databaseOperations, IMessageHandlerResolver resolver)
        {
            _databaseOperations = databaseOperations;
            _resolver = resolver;
        }

        public void Start()
        {
            _started = true;
            new Thread(() =>
            {
                while (_started)
                {
                    try
                    {
                        var item = _databaseOperations.Peek();
                        if (item == null)
                        {
                            Thread.Sleep(500);
                            continue;
                        }

                        _databaseOperations.Dequeue(item).RunSynchronously();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }).Start();
        }
        

        public void Stop()
        {
            _started = false;
        }
    }

    public interface IDatabaseOperations
    {
        QueueItem Peek();
        Task Dequeue(QueueItem item);
    }

    public class QueueItem
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public string ContentType { get; set; }
        public string Encoding { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Deferred { get; set; }
        public string Data { get; set; }
    }

    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly string _connectionString;
        private readonly SqlQueries _queries;

        public DatabaseOperations(string connectionString, string schemaName, string tableName)
        {
            _queries = new SqlQueries(schemaName, tableName);
            _connectionString = connectionString;
        }

        public QueueItem Peek()
        {
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
                    item.ContentType = GetValue<string>(reader, Columns.Indexes.ContentTypeIndex);
                    item.Created = GetValue<DateTime>(reader, Columns.Indexes.CreatedIndex);
                    item.Data = GetValue<string>(reader, Columns.Indexes.DataIndex);
                    item.Deferred = GetValue<DateTime?>(reader, Columns.Indexes.DeferredIndex);
                    item.Encoding = GetValue<string>(reader, Columns.Indexes.EncodingIndex);
                    item.MessageId = GetValue<Guid>(reader, Columns.Indexes.MessageIdIndex);
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
                var command = new SqlCommand(_queries.DeQueue);
                command.Parameters.AddWithValue("@Id", item.Id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}

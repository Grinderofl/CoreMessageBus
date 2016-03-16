using System;
using System.Data.SqlClient;
using System.Text;
using CoreMessageBus.ServiceBus.Domain;
using CoreMessageBus.ServiceBus.Internal;
using CoreMessageBus.ServiceBus.SqlServer.Extensions;

namespace CoreMessageBus.ServiceBus.SqlServer.Internal
{
    public class SqlQueueItemFactory
    {
        private readonly IDataSerializer _dataSerializer;

        public SqlQueueItemFactory(IDataSerializer dataSerializer)
        {
            if (dataSerializer == null) throw new ArgumentNullException(nameof(dataSerializer));
            _dataSerializer = dataSerializer;
        }

        public virtual QueueItem Create(SqlDataReader reader)
        {
            if (reader.Read())
            {
                var item = new QueueItem();
                item.Queue = new Queue();
                item.Id = reader.GetValue<Guid>(Columns.Indexes.IdIndex);
                item.MessageId = reader.GetValue<Guid>(Columns.Indexes.MessageIdIndex);

                item.Created = reader.GetValue<DateTime>(Columns.Indexes.CreatedIndex);
                item.Deferred = reader.GetValue<DateTime?>(Columns.Indexes.DeferredIndex);

                item.ContentType = reader.GetValue<string>(Columns.Indexes.ContentTypeIndex);


                var typeString = reader.GetValue<string>(Columns.Indexes.TypeIndex);
                item.Type = Type.GetType(typeString, false);

                var dataAsString = reader.GetValue<string>(Columns.Indexes.DataIndex);
                item.Data = _dataSerializer.Deserialize(dataAsString, item.Type);

                var encodingAsString = reader.GetValue<string>(Columns.Indexes.EncodingIndex);
                item.Encoding = Encoding.GetEncoding(encodingAsString);

                item.Queue.Id = reader.GetValue<int>(Columns.Indexes.QueueIdIndex);
                item.Queue.Name = reader.GetValue<string>(Columns.Indexes.QueueNameIndex);

                return item;
            }
            return null;
        }
    }
}
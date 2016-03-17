using System;
using System.Data.SqlClient;

namespace CoreMessageBus.ServiceBus.SqlServer.Extensions
{
    public static class SqlParameterCollectionExtensions
    {
        public static SqlParameterCollection AddParameter(this SqlParameterCollection collection, string column, object value)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.AddWithValue(column, value ?? DBNull.Value);
            return collection;
        }
    }
}
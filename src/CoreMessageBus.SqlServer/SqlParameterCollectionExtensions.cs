using System;
using System.Data.SqlClient;

namespace CoreMessageBus.SqlServer
{
    public static class SqlParameterCollectionExtensions
    {
        public static SqlParameterCollection AddParameter(this SqlParameterCollection collection, string column, object value)
        {
            collection.AddWithValue(column, value ?? DBNull.Value);
            return collection;
        }
    }
}
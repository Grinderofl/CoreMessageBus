using System;
using System.Data.SqlClient;

namespace CoreMessageBus.ServiceBus.SqlServer.Extensions
{
    public static class SqlReaderExtensions
    {
        public static T GetValue<T>(this SqlDataReader reader, int index)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!reader.IsDBNull(index))
                return reader.GetFieldValue<T>(index);
            return default(T);
        }
    }
}
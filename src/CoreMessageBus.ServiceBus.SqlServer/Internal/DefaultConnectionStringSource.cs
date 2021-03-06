using System;
using CoreMessageBus.ServiceBus.SqlServer.Configuration;

namespace CoreMessageBus.ServiceBus.SqlServer.Internal
{
    public class DefaultConnectionStringSource : IConnectionStringSource
    {
        private readonly SqlServerQueueOperationOptions _options;

        public DefaultConnectionStringSource(SqlServerQueueOperationOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            _options = options;
        }

        public string GetConnectionString() => _options.ConnectionStringValue;
    }
}
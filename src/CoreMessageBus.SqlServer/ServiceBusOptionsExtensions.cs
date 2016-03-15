using CoreMessageBus.ServiceBus;

namespace CoreMessageBus.SqlServer
{
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
}
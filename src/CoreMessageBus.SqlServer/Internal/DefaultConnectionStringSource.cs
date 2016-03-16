namespace CoreMessageBus.SqlServer.Internal
{
    public class DefaultConnectionStringSource : IConnectionStringSource
    {
        private readonly string _connectionString;

        public DefaultConnectionStringSource(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString() => _connectionString;



    }
}
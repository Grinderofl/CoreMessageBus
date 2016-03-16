using System.Data.SqlClient;

namespace CoreMessageBus.ServiceBus.SqlServer.Internal
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConnectionStringSource _connectionStringSource;

        public SqlConnectionFactory(IConnectionStringSource connectionStringSource)
        {
            _connectionStringSource = connectionStringSource;
        }

        public SqlConnection Create()
        {
            return new SqlConnection(_connectionStringSource.GetConnectionString());
        }
    }
}
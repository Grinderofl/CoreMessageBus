using System.Data.SqlClient;

namespace CoreMessageBus.ServiceBus.SqlServer.Internal
{
    public interface ISqlConnectionFactory
    {
        SqlConnection Create();
    }
}
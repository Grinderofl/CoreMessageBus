using System.Data.SqlClient;

namespace CoreMessageBus.SqlServer.Internal
{
    public interface ISqlConnectionFactory
    {
        SqlConnection Create();
    }
}
namespace CoreMessageBus.ServiceBus.SqlServer.Internal
{
    public interface IConnectionStringSource
    {
        string GetConnectionString();
    }
}
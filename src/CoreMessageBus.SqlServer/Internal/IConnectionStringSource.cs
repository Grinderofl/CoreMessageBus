namespace CoreMessageBus.SqlServer.Internal
{
    public interface IConnectionStringSource
    {
        string GetConnectionString();
    }
}
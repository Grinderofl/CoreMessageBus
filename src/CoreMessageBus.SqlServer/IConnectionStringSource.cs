namespace CoreMessageBus.SqlServer
{
    public interface IConnectionStringSource
    {
        string GetConnectionString();
    }
}
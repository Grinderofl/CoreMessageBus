namespace CoreMessageBus.ServiceBus.Internal
{
    public interface IServiceBusUnitOfWork
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}
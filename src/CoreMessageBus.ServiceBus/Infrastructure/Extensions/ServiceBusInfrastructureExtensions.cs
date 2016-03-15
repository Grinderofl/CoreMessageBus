namespace CoreMessageBus.ServiceBus.Infrastructure.Extensions
{
    public static class ServiceBusInfrastructureExtensions
    {
        public static IServiceBusInfrastructure GetInfrastructure<T>(this T infrastructure) where T : IServiceBusInfrastructure
        {
            return (IServiceBusInfrastructure) infrastructure;
        }
    }
}
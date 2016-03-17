using System;

namespace CoreMessageBus.ServiceBus.Infrastructure.Extensions
{
    public static class ServiceBusInfrastructureExtensions
    {
        public static IServiceBusInfrastructure GetInfrastructure<T>(this T infrastructure) where T : IServiceBusInfrastructure
        {
            if (infrastructure == null) throw new ArgumentNullException(nameof(infrastructure));
            return (IServiceBusInfrastructure) infrastructure;
        }
    }
}
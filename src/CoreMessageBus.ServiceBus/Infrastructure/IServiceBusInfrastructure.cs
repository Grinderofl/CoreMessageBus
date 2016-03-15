using Microsoft.Extensions.DependencyInjection;

namespace CoreMessageBus.ServiceBus.Infrastructure
{
    public interface IServiceBusInfrastructure
    {
        IServiceCollection Services { get; }
    }
}
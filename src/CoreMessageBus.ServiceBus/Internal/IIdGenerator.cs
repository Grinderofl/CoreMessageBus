using System;

namespace CoreMessageBus.ServiceBus.Internal
{
    public interface IIdGenerator
    {
        Guid Create();
    }
}
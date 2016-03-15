using System;

namespace CoreMessageBus.ServiceBus
{
    public interface IIdGenerator
    {
        Guid Create();
    }
}
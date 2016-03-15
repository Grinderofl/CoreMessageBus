using System;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class IdGenerator : IIdGenerator
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
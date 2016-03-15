using System;

namespace CoreMessageBus.ServiceBus
{
    public class IdGenerator : IIdGenerator
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
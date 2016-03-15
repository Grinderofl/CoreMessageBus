using System;

namespace CoreMessageBus.ServiceBus
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
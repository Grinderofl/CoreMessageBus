using System;

namespace CoreMessageBus.ServiceBus.Internal
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
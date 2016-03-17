using System;
using CoreMessageBus.Internal;
using JetBrains.Annotations;

namespace CoreMessageBus.Configuration
{
    public class MessageBusConfiguration
    {
        internal readonly MessageHandlerRegistry Registry = new MessageHandlerRegistry();

        public MessageBusConfiguration RegisterHandler<T>()
        {
            Registry.RegisterHandler<T>();
            return this;
        }

        public MessageBusConfiguration RegisterHandler([NotNull] Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));
            Registry.RegisterHandler(handlerType);
            return this;
        }
    }
}
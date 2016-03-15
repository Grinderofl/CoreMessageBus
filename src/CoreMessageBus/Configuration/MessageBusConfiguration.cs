using System;
using CoreMessageBus.Internal;

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

        public MessageBusConfiguration RegisterHandler(Type handlerType)
        {
            Registry.RegisterHandler(handlerType);
            return this;
        }
    }
}
using System;

namespace CoreMessageBus.ServiceBus
{
    public interface IServiceBus
    {
        /// <summary>
        /// Sends a message to service bus for handling
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="message">Message to send</param>
        void Send<TMessage>(TMessage message);

        void Defer<TMessage>(TMessage message, TimeSpan timespan);
        void Defer<TMessage>(TMessage message, DateTime until);
    }
}
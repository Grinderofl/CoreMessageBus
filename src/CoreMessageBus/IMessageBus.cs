using System;
using System.Threading.Tasks;

namespace CoreMessageBus
{
    public interface IMessageBus
    {
        /// <summary>
        /// Sends a message to message bus for handling
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="message">Message to send</param>
        void Send<TMessage>(TMessage message);

        /// <summary>
        /// Sends a message to message bus for asynchronous handling
        /// </summary>
        /// <typeparam name="TMessage">Message type</typeparam>
        /// <param name="message">Message to send</param>
        /// <returns></returns>
        Task SendAsync<TMessage>(TMessage message);
    }

    public interface IServiceBus : IMessageBus
    {
        void Defer<TMessage>(TMessage message, TimeSpan timespan);
        void Defer<TMessage>(TMessage message, DateTime until);
    }

    public interface IMessageQueueProcessor
    {
        void Start();
        void Stop();
    }
}
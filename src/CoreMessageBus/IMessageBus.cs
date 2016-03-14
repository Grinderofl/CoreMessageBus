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
}
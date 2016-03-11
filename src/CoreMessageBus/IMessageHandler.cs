namespace CoreMessageBus
{
    /// <summary>
    /// Handles incoming messages
    /// </summary>
    /// <typeparam name="TMessage">Type of message to handle</typeparam>
    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}
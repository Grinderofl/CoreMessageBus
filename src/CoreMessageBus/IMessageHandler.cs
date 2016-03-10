namespace CoreMessageBus
{
    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}
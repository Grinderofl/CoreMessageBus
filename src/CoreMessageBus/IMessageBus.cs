using System.Threading.Tasks;

namespace CoreMessageBus
{
    public interface IMessageBus
    {
        void Send<TMessage>(TMessage message);
        Task SendAsync<TMessage>(TMessage message);
    }
}
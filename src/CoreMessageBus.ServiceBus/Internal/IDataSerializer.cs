using System;
using System.Text;

namespace CoreMessageBus.ServiceBus.Internal
{
    public interface IDataSerializer
    {
        string ContentType { get; }
        Encoding Encoding { get; }
        object Serialize<TMessage>(TMessage message);
        object Deserialize(string item, Type target);
    }
}
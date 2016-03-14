using CoreMessageBus;

namespace ConsoleSample
{
    public class Message : IMessage
    {
        public string Name { get; set; }
    }
}
using System;
using CoreMessageBus;

namespace ConsoleSample
{
    public class MessageHandlerOne : IMessageHandler<Message>
    {
        public void Handle(Message message)
        {
            Console.WriteLine("{0} world", message.Name);
        }
    }
}
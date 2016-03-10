using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMessageBus;

namespace ConsoleSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var registry = new MessageHandlerRegistry();

            registry.RegisterHandler<MessageHandlerOne>();
            registry.RegisterHandler<MessageHandlerOne>();
        }

        private class Message : IMessage
        {
        }

        private class MessageHandlerOne : IMessageHandler<Message>
        {
            public void Handle(Message message)
            {
                throw new NotImplementedException();
            }
        }
    }
}

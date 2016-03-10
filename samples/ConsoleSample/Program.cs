using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection().AddSingleton<MyDependency>().BuildServiceProvider();
            var factory = new MessageHandlerFactory(serviceProvider);

            var item = factory.Create<MyMessage>(typeof(MyMessageHandler));
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

        private class MyMessageHandler : IMessageHandler<MyMessage>
        {
            public MyMessageHandler(MyDependency dependency)
            {

            }

            public void Handle(MyMessage message)
            {
                throw new NotImplementedException();
            }
        }

        private class MyDependency
        {

        }

        private class MyMessage
        {
        }
    }

    
}

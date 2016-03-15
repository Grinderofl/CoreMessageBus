using System;
using CoreMessageBus.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreMessageBus.Tests
{
    public class MessageHandlerFactoryTests
    {
        [Fact]
        public void Creates_item_using_service_provider()
        {
            var factory = CreateFactory();

            var item = factory.Create<Message>(typeof(MessageHandler));

            Assert.NotNull(item);
        }

        private static MessageHandlerFactory CreateFactory()
        {
            var serviceProvider = new ServiceCollection().AddSingleton<Dependency>().BuildServiceProvider();
            var factory = new MessageHandlerFactory(serviceProvider);
            return factory;
        }

        private class MessageHandler : IMessageHandler<Message>
        {
            public MessageHandler(Dependency dependency)
            {
                
            }

            public void Handle(Message message)
            {
                throw new NotImplementedException();
            }
        }

        private class Dependency
        {
        }

        private class Message
        {
        }
    }
}
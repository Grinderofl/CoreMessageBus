using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace CoreMessageBus.Tests
{
    public class MessageHandlerResolverTests
    {
        [Fact]
        public void Can_resolve()
        {
            var registry = new Mock<MessageHandlerRegistry>();
            var factory = new Mock<IMessageHandlerFactory>();

            var handler = new MessageHandler();

            registry.Setup(x => x.HandlersFor<Message>()).Returns(new [] {typeof(MessageHandler)});
            factory.Setup(x => x.Create<Message>(It.IsAny<Type>())).Returns(handler);

            var resolver = new MessageHandlerResolver(factory.Object, registry.Object);

            var resolved = resolver.Resolve<Message>().FirstOrDefault();

            Assert.Same(handler, resolved);
        }

        private class Message
        {
        }

        private class MessageHandler : IMessageHandler<Message>
        {
            public void Handle(Message message)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
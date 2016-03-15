using System;
using System.Linq;
using CoreMessageBus.Internal;
using Moq;
using Xunit;

namespace CoreMessageBus.Tests
{
    public class CachingMessageHandlerResolverTests
    {
        [Fact]
        public void Can_cache()
        {
            var handler = new MessageHandler();
            var factory = new Mock<IMessageHandlerFactory>();
            factory.Setup(x => x.Create<Message>(It.IsAny<Type>())).Returns(handler);

            var resolver = CreateResolver(factory);

            var resolved = resolver.Resolve<Message>().FirstOrDefault();
            var resolved2 = resolver.Resolve<Message>().FirstOrDefault();

            Assert.Same(handler, resolved);
            Assert.Same(handler, resolved2);
            factory.Verify(x => x.Create<Message>(It.IsAny<Type>()), Times.Once);
        }

        private static CachingMessageHandlerResolver CreateResolver(Mock<IMessageHandlerFactory> factory)
        {
            var registry = new Mock<MessageHandlerRegistry>();
            registry.Setup(x => x.HandlersFor<Message>()).Returns(new[] { typeof(MessageHandler) });

            return new CachingMessageHandlerResolver(factory.Object, registry.Object);
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
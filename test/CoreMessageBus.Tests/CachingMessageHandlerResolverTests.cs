using System;
using System.Linq;
using Moq;
using Xunit;

namespace CoreMessageBus.Tests
{
    public class CachingMessageHandlerResolverTests
    {
        [Fact]
        public void Can_cache()
        {
            var registry = new Mock<MessageHandlerRegistry>();
            var factory = new Mock<IMessageHandlerFactory>();

            var handler = new MessageHandler();

            registry.Setup(x => x.HandlersFor<Message>()).Returns(new [] {typeof(MessageHandler)});
            factory.Setup(x => x.Create<Message>(It.IsAny<Type>())).Returns(handler);

            var resolver = new CachingMessageHandlerResolver(factory.Object, registry.Object, TODO);

            var resolved = resolver.Resolve<Message>().FirstOrDefault();
            var resolved2 = resolver.Resolve<Message>().FirstOrDefault();

            Assert.Same(handler, resolved);
            Assert.Same(handler, resolved2);
            factory.Verify(x => x.Create<Message>(It.IsAny<Type>()), Times.Once);
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
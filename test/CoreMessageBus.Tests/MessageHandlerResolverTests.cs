using System;
using System.Collections.Generic;
using System.Linq;
using CoreMessageBus.Annotations;
using CoreMessageBus.Internal;
using Moq;
using Xunit;

namespace CoreMessageBus.Tests
{
    public class MessageHandlerResolverTests
    {
        [Fact]
        public void Can_resolve()
        {
            var handler = new MessageHandler();
            var resolver = GetResolver(handler);

            var resolved = resolver.Resolve<Message>().FirstOrDefault();

            Assert.Same(handler, resolved);
        }

        private static MessageHandlerResolver GetResolver(MessageHandler handler)
        {
            var registry = new Mock<MessageHandlerRegistry>();
            var factory = new Mock<IMessageHandlerFactory>();

            registry.Setup(x => x.HandlersFor<Message>()).Returns(new[] { typeof(MessageHandler) });
            factory.Setup(x => x.Create<Message>(It.IsAny<Type>())).Returns(handler);

            return new MessageHandlerResolver(factory.Object, registry.Object);
        }

        [UsedImplicitly]
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
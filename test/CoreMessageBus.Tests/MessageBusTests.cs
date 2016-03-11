using Moq;
using Xunit;

namespace CoreMessageBus.Tests
{
    public class MessageBusTests
    {
        [Fact]
        public void Can_send_messages()
        {
            var messageBus = CreateMessageBus();
            var message = new Message();
            messageBus.Send(message);
            Assert.Equal("Hello", message.Value);
        }

        private IMessageBus CreateMessageBus()
        {
            var resolver = new Mock<IMessageHandlerResolver>();
            resolver.Setup(x => x.Resolve<Message>()).Returns(new[] {new MessageHandler()});
            return new MessageBus(resolver.Object);
        }

        private class Message
        {
            public string Value { get; set; }
        }

        private class MessageHandler : IMessageHandler<Message>
        {
            public void Handle(Message message)
            {
                message.Value = "Hello";
            }
        }
    }
}
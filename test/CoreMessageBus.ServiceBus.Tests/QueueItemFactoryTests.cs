using System;
using CoreMessageBus.ServiceBus.Domain;
using Moq;
using Xunit;

namespace CoreMessageBus.ServiceBus.Tests
{
    public class QueueItemFactoryTests
    {
        [Fact]
        public void Creates_queueitem()
        {

            var qsMock = new Mock<IQueueSelector>();
            var queue = new Queue(1, "Queue");
            qsMock.Setup(x => x.GetQueue<TestMessage>()).Returns(queue);
            var factory = new QueueItemFactory(new JsonDataSerializer(), new DateTimeProvider(), new IdGenerator(),
                qsMock.Object);
            var item = factory.Create(new TestMessage());
            Assert.NotEqual(Guid.Empty, item.Id);
            Assert.NotEqual(Guid.Empty, item.MessageId);
            Assert.NotNull(item.Data);
        }

        private class TestMessage
        {
        }
    }
}
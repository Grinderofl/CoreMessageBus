using CoreMessageBus.ServiceBus.Domain;
using CoreMessageBus.ServiceBus.Queue;
using Moq;
using Xunit;

namespace CoreMessageBus.ServiceBus.Tests
{
    public class ServiceBusTests
    {
        [Fact]
        public void Queues_items()
        {
            var qoMock = new Mock<IQueueOperations>();
            var qifMock = new Mock<IQueueItemFactory>();
            qifMock.Setup(x => x.Create<TestMessage>(It.IsAny<TestMessage>(), null)).Returns(new QueueItem());

            var bus = new ServiceBus(qoMock.Object, qifMock.Object);

            bus.Send(new TestMessage());

            qoMock.Verify(x => x.Queue(It.IsAny<QueueItem>()));
        }

        private class TestMessage
        {
            
        }
    }
}
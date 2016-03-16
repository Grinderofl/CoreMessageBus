using System.Collections.Generic;
using System.Linq;
using CoreMessageBus.ServiceBus.Configuration;
using CoreMessageBus.ServiceBus.Domain;
using CoreMessageBus.ServiceBus.Internal;
using Moq;
using Xunit;

namespace CoreMessageBus.ServiceBus.Tests
{
    public class QueueSelectorTests
    {
        [Fact]
        public void Gets_queue()
        {
            var options = new QueueOptions().Queue("queue", new[] {typeof (TestMessage)});
            var sbMock = new Mock<IServiceBusQueue>();
            sbMock.Setup(x => x.GetQueueId("queue")).Returns(1);
            var queueSelector = new QueueSelector(options, sbMock.Object);

            var queue = queueSelector.GetQueue<TestMessage>();
            Assert.Equal(1, queue.Id);
            Assert.Equal("queue", queue.Name);
        }

        [Fact]
        public void Gets_queues()
        {
            var qoMock = new Mock<QueueOptions>();
            var sbMock = new Mock<IServiceBusQueue>();
            sbMock.Setup(x => x.GetQueues()).Returns(new List<Queue>() {new Queue(1, "queue1"), new Queue(2, "queue2")});

            var queueSelector = new QueueSelector(qoMock.Object, sbMock.Object);

            var queues = queueSelector.GetQueues();
            Assert.Equal(2, queues.Count());
        }

        private class TestMessage
        {
             
        }
    }
}
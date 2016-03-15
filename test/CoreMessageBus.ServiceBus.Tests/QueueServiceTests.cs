using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMessageBus.ServiceBus.Domain;
using Moq;
using Xunit;

namespace CoreMessageBus.ServiceBus.Tests
{
    public class QueueServiceTests
    {
        [Fact]
        public void Processes_items()
        {
            var qoMock = new Mock<IQueueOperations>();
            var mbMock = new Mock<IMessageBus>();

            qoMock.Setup(x => x.Peek()).Returns(new QueueItem() {Data = new TestMessage(), Type = typeof (TestMessage)});

            var service = new QueueService(qoMock.Object, mbMock.Object);

            service.ProcessNextItem();

            mbMock.Verify(x => x.Send(It.IsAny<TestMessage>()), Times.Once);
        }

        [Fact]
        public void Updates_queue_on_success()
        {
            var qoMock = new Mock<IQueueOperations>();
            var mbMock = new Mock<IMessageBus>();

            qoMock.Setup(x => x.Peek()).Returns(new QueueItem() {Data = new TestMessage(), Type = typeof (TestMessage)});

            var service = new QueueService(qoMock.Object, mbMock.Object);

            service.ProcessNextItem();

            qoMock.Verify(x => x.Success(It.IsAny<QueueItem>()), Times.Once);
        }

        [Fact]
        public void Updates_queue_on_error()
        {
            var qoMock = new Mock<IQueueOperations>();
            var mbMock = new Mock<IMessageBus>();
            mbMock.Setup(x => x.Send(It.IsAny<TestMessage>())).Throws<MessageBusException>();
            qoMock.Setup(x => x.Peek())
                .Returns(new QueueItem() {Data = new TestMessage(), Type = typeof (TestMessage), Id = Guid.Empty});

            var service = new QueueService(qoMock.Object, mbMock.Object);

            service.ProcessNextItem();

            qoMock.Verify(x => x.Error(It.IsAny<MessageBusException>(), Guid.Empty), Times.Once);
        }

        private class TestMessage
        {

        }
    }
}


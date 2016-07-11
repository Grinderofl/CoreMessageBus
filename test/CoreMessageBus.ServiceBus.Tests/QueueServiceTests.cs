using System;
using System.Reflection;
using System.Threading.Tasks;
using CoreMessageBus.ServiceBus.Domain;
using CoreMessageBus.ServiceBus.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CoreMessageBus.ServiceBus.Tests
{
    public class QueueServiceTests
    {
        [Fact]
        public void Processes_items()
        {
            var qoMock = new Mock<IServiceBusQueue>();
            var mbMock = new Mock<IMessageBus>();
            //var ssfMock = new Mock<IServiceScopeFactory>();
            //var ssMock = new Mock<IServiceScope>();
            //var spMock = new Mock<IServiceProvider>();
            //var lMock = new Mock<ILogger>();

            //ssfMock.Setup(x => x.CreateScope()).Returns(ssMock.Object);
            //ssMock.SetupGet(x => x.ServiceProvider).Returns(spMock.Object);
            //spMock.Setup(x => x.GetService<IMessageBus>());

            qoMock.Setup(x => x.Peek()).Returns(new QueueItem() {Data = new TestMessage(), Type = typeof (TestMessage)});

            var service = CreateQueueService(qoMock.Object, mbMock.Object);// new QueueService(qoMock.Object, ssfMock.Object, new ServiceBusUnitOfWork(), lMock.Object);

            service.ProcessNextItem();

            mbMock.Verify(x => x.Send(It.IsAny<TestMessage>()), Times.Once);
        }

        private QueueService CreateQueueService(IServiceBusQueue queue, IMessageBus messageBus)
        {
            queue = queue ?? new Mock<IServiceBusQueue>().Object;
            messageBus = messageBus ?? new Mock<IMessageBus>().Object;

            var ssfMock = new Mock<IServiceScopeFactory>();
            var ssMock = new Mock<IServiceScope>();
            var spMock = new Mock<IServiceProvider>();
            var lMock = new Mock<ILogger>();

            ssfMock.Setup(x => x.CreateScope()).Returns(ssMock.Object);
            ssMock.SetupGet(x => x.ServiceProvider).Returns(spMock.Object);
            spMock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(messageBus);

            return new QueueService(queue, ssfMock.Object, new ServiceBusUnitOfWork(), lMock.Object);
        }

        [Fact]
        public void Updates_queue_on_success()
        {
            var qoMock = new Mock<IServiceBusQueue>();

            qoMock.Setup(x => x.Peek()).Returns(new QueueItem() {Data = new TestMessage(), Type = typeof (TestMessage)});

            var service = CreateQueueService(qoMock.Object, null);

            service.ProcessNextItem();

            qoMock.Verify(x => x.Success(It.IsAny<QueueItem>()), Times.Once);
        }

        public Delegate MethodThatThrows;

        [Fact]
        public void Updates_queue_on_error()
        {
            var qoMock = new Mock<IServiceBusQueue>();
            //var mbMock = new MockMessageBus();
            var mbMock = new Mock<IMessageBus>();
            mbMock.Setup(x => x.Send(It.IsAny<TestMessage>())).Throws<MessageBusException>();
            qoMock.Setup(x => x.Peek())
                .Returns(new QueueItem() {Data = new TestMessage(), Type = typeof (TestMessage), Id = Guid.Empty});

            var service = CreateQueueService(qoMock.Object, mbMock.Object);


            Action @delegate = () => service.ProcessNextItem();

            Assert.Throws<TargetInvocationException>(@delegate);
            qoMock.Verify(x => x.Error(It.IsAny<MessageBusException>(), Guid.Empty), Times.Once);
        }

        private class MockMessageBus : IMessageBus
        {
            public void Send<TMessage>(TMessage message)
            {
                if(typeof(TMessage) == typeof(TestMessage))
                    throw new MessageBusException();

            }

            public Task SendAsync<TMessage>(TMessage message)
            {
                throw new NotImplementedException();
            }
        }

        private class TestMessage
        {

        }
    }
}


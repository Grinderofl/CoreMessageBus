using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;

namespace CoreMessageBus.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class MessageHandlerRegistryTests
    {
        [Fact]
        public void Adds_handlers_only_once()
        {
            var registry = new MessageHandlerRegistry();

            registry.RegisterHandler<MessageHandler>();
            registry.RegisterHandler<MessageHandler>();

            Assert.Equal(1, registry.RegistryItems.Count);
        }

        [UsedImplicitly]
        private class Message : IMessage
        {
        }

        [UsedImplicitly]
        private class MessageHandler : IMessageHandler<Message>
        {
            public void Handle(Message message)
            {
                throw new NotImplementedException();
            }
        }
    }
}

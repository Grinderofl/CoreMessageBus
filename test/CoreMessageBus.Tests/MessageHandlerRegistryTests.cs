using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private class Message : IMessage
        {
        }

        private class MessageHandler : IMessageHandler<Message>
        {
            public void Handle(Message message)
            {
                throw new NotImplementedException();
            }
        }
    }

    public class MessageHandlerCacheTests
    {
        [Fact]
        public void Can_cache_items()
        {
            var cache = new MessageHandlerCache();

            cache.Add(new MessageHandler());
            Assert.Equal(1, cache.CacheItems.Count);
        }

        private class Message : IMessage
        {
        }

        private class MessageHandler : IMessageHandler<Message>
        {
            public void Handle(Message message)
            {
                throw new NotImplementedException();
            }
        }
    }
}

using System;
using Xunit;

namespace CoreMessageBus.Tests
{
    public class MessageHandlerCacheTests
    {
        [Fact]
        public void Can_cache_items_without_duplicates()
        {
            var cache = new MessageHandlerCache();

            cache.Add(new MessageHandler());
            cache.Add(new MessageHandler());
            Assert.Equal(1, cache.CacheItems.Count);
        }

        [UsedImplicitly]
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
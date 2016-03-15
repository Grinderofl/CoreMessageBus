using System;
using System.Linq;
using System.Threading.Tasks;
using CoreMessageBus.Annotations;
using CoreMessageBus.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace CoreMessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly IMessageHandlerResolver _resolver;
        private readonly ILogger<MessageBus> _logger;

        public MessageBus([NotNull]IMessageHandlerResolver resolver, [CanBeNull] ILogger<MessageBus> logger = null)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            _resolver = resolver;
            _logger = logger;
        }


        public void Send<TMessage>(TMessage message)
        {
            _logger?.LogTrace($"Attempting to resolve handlers for {typeof(TMessage)}");
            var handlers = _resolver.Resolve<TMessage>().ToList();
            _logger?.LogTrace($"Resolved {handlers.Count()} handlers for {typeof(TMessage)}");
            foreach (var handler in handlers)
            {
                _logger?.LogDebug($"Executing handler {handler.GetType()}");
                try
                {
                    handler.Handle(message);
                }
                catch (Exception ex)
                {
                    throw new MessageBusException($"Message handler failed while executing {handler.GetType()}", ex);
                }
            }
        }

        // TODO: Make actually async
        public Task SendAsync<TMessage>(TMessage message)
        {
            return Task.Run(() =>
            {
                Send(message);
            });
        }
    }
}

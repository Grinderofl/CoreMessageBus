using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
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
                _logger?.LogTrace($"Executing handler {handler.GetType()}");
                handler.Handle(message);
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

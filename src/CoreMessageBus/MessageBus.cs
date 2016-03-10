using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CoreMessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly IMessageHandlerResolver _resolver;

        public MessageBus([NotNull]IMessageHandlerResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            _resolver = resolver;
        }


        public void Send<TMessage>(TMessage message)
        {
            var handlers = _resolver.Resolve<TMessage>();
            foreach (var handler in handlers)
                handler.Handle(message);
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

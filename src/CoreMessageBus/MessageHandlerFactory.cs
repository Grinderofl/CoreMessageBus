using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMessageBus
{
    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MessageHandlerFactory([NotNull] IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            _serviceProvider = serviceProvider;
        }

        public IMessageHandler<TMessage> Create<TMessage>(Type handlerType)
        {
            var instance = ActivatorUtilities.CreateInstance(_serviceProvider, handlerType);
            return instance as IMessageHandler<TMessage>;
        }
    }
}
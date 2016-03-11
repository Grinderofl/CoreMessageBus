using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace CoreMessageBus
{
    public class MessageHandlerResolver : IMessageHandlerResolver
    {
        private readonly IMessageHandlerFactory _factory;
        private readonly MessageHandlerRegistry _registry;
        protected readonly ILogger Logger;

        public MessageHandlerResolver([NotNull]IMessageHandlerFactory factory, [NotNull] MessageHandlerRegistry registry, [CanBeNull]ILogger logger = null)
        {
            
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (registry == null) throw new ArgumentNullException(nameof(registry));
            _factory = factory;
            _registry = registry;
            Logger = logger;
        }

        public IEnumerable<IMessageHandler<TMessage>> Resolve<TMessage>()
        {
            Logger?.LogTrace($"Retrieving handlers for {typeof(TMessage)}");
            var resolvedHandlerTypes = _registry.HandlersFor<TMessage>().ToList();
            Logger?.LogTrace($"Found {resolvedHandlerTypes.Count()} handlers for {typeof (TMessage)}");
            foreach (var resolvedHandlerType in resolvedHandlerTypes)
            {
                yield return ResolveHandlerType<TMessage>(resolvedHandlerType); ;
            }
        }

        protected virtual IMessageHandler<TMessage> ResolveHandlerType<TMessage>(Type resolvedHandlerType)
        {
            Logger?.LogDebug($"Resolving {resolvedHandlerType} using factory");
            return _factory.Create<TMessage>(resolvedHandlerType);
        }
    }
}
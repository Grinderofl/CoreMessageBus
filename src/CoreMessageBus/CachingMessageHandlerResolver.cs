using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace CoreMessageBus
{
    public class CachingMessageHandlerResolver : MessageHandlerResolver
    {
        private readonly MessageHandlerCache _cache = new MessageHandlerCache();

        public CachingMessageHandlerResolver([NotNull] IMessageHandlerFactory factory, [NotNull] MessageHandlerRegistry registry,[CanBeNull] ILogger logger = null) : base(factory, registry, logger)
        {
        }

        protected override IMessageHandler<TMessage> ResolveHandlerType<TMessage>(Type resolvedHandlerType)
        {
            Logger?.LogTrace($"Checking cache for {resolvedHandlerType}");
            if (_cache.Contains(resolvedHandlerType))
            {
                Logger?.LogDebug($"Resolving {resolvedHandlerType} using cache");
                return _cache.Get<TMessage>(resolvedHandlerType);
            }

            var handler = base.ResolveHandlerType<TMessage>(resolvedHandlerType);
            Logger?.LogTrace($"Adding {resolvedHandlerType} to cache");
            _cache.Add(handler);
            return handler;
        }
    }
}
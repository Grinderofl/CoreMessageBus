using System;
using System.Collections.Generic;

namespace CoreMessageBus.Internal
{
    public class MessageHandlerRegistryItem<TType> : MessageHandlerRegistryItem
    {
        public MessageHandlerRegistryItem() : base(typeof(TType))
        {
        }
    }

    public class MessageHandlerRegistryItem
    {

        public MessageHandlerRegistryItem(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            MessageType = type;
        }

        public Type MessageType { get; set; }
        public ISet<Type> MessageHandlers { get; set; } = new HashSet<Type>();

        public bool Handles<T>() => MessageType == typeof (T);
        public bool Handles(Type type) => MessageType == type;
    }
}
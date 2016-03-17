using System;
using System.Collections.Generic;
using System.Threading;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class QueueProcessorCache
    {
        private static readonly object Lock = new object();

        private static readonly ISet<Guid> CurrentlyProcessingIds = new HashSet<Guid>();

        public bool Contains(QueueItem item)
        {
            bool contains;
            lock (Lock)
            {
                contains = CurrentlyProcessingIds.Contains(item.Id);
            }
            return contains;
        }

        public void Add(QueueItem item)
        {
            lock (Lock)
            {
                if (!CurrentlyProcessingIds.Contains(item.Id))
                {
                    CurrentlyProcessingIds.Add(item.Id);
                }
            }   
        }

        public void Remove(QueueItem item)
        {

            lock (Lock)
            {
                if (CurrentlyProcessingIds.Contains(item.Id))
                {
                    CurrentlyProcessingIds.Remove(item.Id);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreMessageBus.ServiceBus
{
    public class QueueOptions
    {
        public IDictionary<string, ISet<Type>> HandlesQueues { get; protected set; } = new Dictionary<string, ISet<Type>>();
        public int SleepTime { get; set; } = 1000;

        public QueueOptions Queue(string queueName, IEnumerable<Type> types)
        {
            if(!HandlesQueues.ContainsKey(queueName))
                HandlesQueues.Add(queueName, new HashSet<Type>());
            HandlesQueues[queueName] = new HashSet<Type>(HandlesQueues[queueName].Concat(types).Distinct());
            return this;
        }
    }
}
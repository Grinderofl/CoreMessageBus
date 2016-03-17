using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreMessageBus.ServiceBus.Configuration
{
    public class QueueOptions
    {
        public IDictionary<string, ISet<Type>> HandlesQueues { get; protected set; } = new Dictionary<string, ISet<Type>>();
        public int SleepTime { get; set; } = 1000;

        public int Workers { get; set; } = 1;

        public QueueOptions Queue(string queueName, IEnumerable<Type> types)
        {
            if (queueName == null) throw new ArgumentNullException(nameof(queueName));
            if (types == null) throw new ArgumentNullException(nameof(types));
            if(!HandlesQueues.ContainsKey(queueName))
                HandlesQueues.Add(queueName, new HashSet<Type>());
            HandlesQueues[queueName] = new HashSet<Type>(HandlesQueues[queueName].Concat(types).Distinct());
            return this;
        }

        
    }
}
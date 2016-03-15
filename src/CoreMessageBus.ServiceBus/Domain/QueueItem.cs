using System;
using System.Text;

namespace CoreMessageBus.ServiceBus.Domain
{
    public class QueueItem
    {
        public Guid Id { get; set; }
        public Guid MessageId { get; set; }
        public string ContentType { get; set; }
        public Encoding Encoding { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Deferred { get; set; }
        public object Data { get; set; }
        public Type Type { get; set; }
        public Queue Queue { get; set; }
    }
}
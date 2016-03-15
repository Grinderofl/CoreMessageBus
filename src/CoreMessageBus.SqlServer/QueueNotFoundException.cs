using System;

namespace CoreMessageBus.SqlServer
{
    public class QueueNotFoundException : Exception
    {
        public QueueNotFoundException()
        {
        }

        public QueueNotFoundException(string message) : base(message)
        {
        }

        public QueueNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
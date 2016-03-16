using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CoreMessageBus.ServiceBus.Domain;

namespace CoreMessageBus.SqlServer.Internal
{
    public interface IDbCommandFactory
    {
        SqlCommand CreatePeekCommand(IEnumerable<Queue> queue);
        SqlCommand CreateDequeueCommand(QueueItem item);
        SqlCommand CreateQueueCommand(QueueItem item);
        SqlCommand CreateQueueIdCommand(string queueName);
        SqlCommand CreateErrorCommand(MessageBusException ex, Guid queueItemId);
        SqlCommand CreateSuccessCommand(QueueItem queueItemId);
        SqlCommand CreateQueuesCommand();
    }
}
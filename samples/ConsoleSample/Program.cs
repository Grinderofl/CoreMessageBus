using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreMessageBus;
using CoreMessageBus.ServiceBus;
using CoreMessageBus.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ConsoleSample
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var settings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
            var queueItem = new QueueItem()
            {
                ContentType = "application/json",
                Created = DateTime.Now,
                Data = new Message() { Name = "Hi"},
                Type = typeof(Message),
                Encoding = Encoding.UTF8,
                MessageId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
            };
            var dataStr = JsonConvert.SerializeObject(queueItem.Data, settings);
            var str = JsonConvert.SerializeObject(queueItem, settings);

            var provider = new ServiceCollection().AddMessageBus(x => x.RegisterHandler<MessageHandlerOne>()).BuildServiceProvider();
            var operations = new SqlServerQueueOperations("Server=.;Database=ServiceBusQueue;Trusted_Connection=True;", "dbo", "SqlServerQueue");
            var processor = new ServiceBusClient(new QueueService(operations, provider.GetService<IMessageBus>()));

            processor.Start();
            Console.ReadKey();
            //var serviceProvider = new ServiceCollection().AddMessageBus(x => x.RegisterHandler<MessageHandlerOne>()).BuildServiceProvider();
            //var bus = serviceProvider.GetService<IMessageBus>();

            //bus.Send(new Message());
        }

       

        private class MyMessageHandler : IMessageHandler<MyMessage>
        {
            public MyMessageHandler(MyDependency dependency)
            {

            }

            public void Handle(MyMessage message)
            {
                throw new NotImplementedException();
            }
        }

        private class MyDependency
        {

        }

        private class MyMessage
        {
        }
    }
}

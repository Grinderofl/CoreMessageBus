using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using CoreMessageBus;
using CoreMessageBus.ServiceBus;
using CoreMessageBus.ServiceBus.Extensions;
using CoreMessageBus.ServiceBus.Tests;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using CoreMessageBus.ServiceBus.SqlServer.Extensions;

namespace ConsoleSample
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            //var tests = new QueueItemFactoryTests();
            //tests.Creates_queueitem();
            //var settings = new JsonSerializerSettings()
            //{
            //    PreserveReferencesHandling = PreserveReferencesHandling.Objects
            //};
            //var queueItem = new QueueItem()
            //{
            //    ContentType = "application/json",
            //    Created = DateTime.Now,
            //    Data = new Message() { Name = "Hi"},
            //    Type = typeof(Message),
            //    Encoding = Encoding.UTF8,
            //    MessageId = Guid.NewGuid(),
            //    Id = Guid.NewGuid(),
            //};
            //var dataStr = JsonConvert.SerializeObject(queueItem.Data, settings);
            //var str = JsonConvert.SerializeObject(queueItem, settings);


            
            //try
            //{
            //    var bus = provider.GetService<IServiceBus>();
            //    bus.Send(new Message() {Name = "World"});
            //}
            //catch (Exception ex)
            //{
                
            //}

            //var operations = new SqlServerQueueOperations("Server=.;Database=ServiceBusQueue;Trusted_Connection=True;", "dbo", "SqlServerQueue");
            //var client = new ServiceBusClient(new QueueService(operations, provider.GetService<IMessageBus>()));
            try
            {
                var provider = new ServiceCollection()
                .AddMessageBus(x => x.RegisterHandler<MessageHandlerOne>())
                .AddServiceBus(x => x
                    .UseSqlServer(s => s.ConnectionString("Server=.;Database=ServiceBusQueue;Trusted_Connection=True;"))
                    .Handles("Queue1", new[] { typeof(Message) })
                )
                .BuildServiceProvider();
                var client = provider.GetService<IServiceBusClient>();
                client.Start();
            }
            catch (Exception e)
            {
                
            }
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

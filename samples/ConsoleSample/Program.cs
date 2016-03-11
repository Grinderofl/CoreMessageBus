﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMessageBus;
using CoreMessageBus.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var operations = new DatabaseOperations("Server=.;Database=ServiceBusQueue;Trusted_Connection=True;", "dbo", "SqlServerQueue");
            var processor = new SqlServerMessageQueueProcessor(operations, null);
            processor.Start();
            Console.ReadKey();
            //var serviceProvider = new ServiceCollection().AddMessageBus(x => x.RegisterHandler<MessageHandlerOne>()).BuildServiceProvider();
            //var bus = serviceProvider.GetService<IMessageBus>();

            //bus.Send(new Message());
        }

        private class Message : IMessage
        {
        }

        private class MessageHandlerOne : IMessageHandler<Message>
        {
            public void Handle(Message message)
            {
                Console.WriteLine("Hello world");
            }
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

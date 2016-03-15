namespace CoreMessageBus.ServiceBus.Domain
{
    public class Queue
    {
        public Queue(int i, string queue)
        {
            Id = i;
            Name = queue;
        }

        public Queue()
        {
            
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
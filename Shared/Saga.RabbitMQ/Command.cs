namespace Saga.RabbitMQ
{
    public abstract record Command
    {
        public Guid MessageGuid { get; set; } = new Guid();
    }
}

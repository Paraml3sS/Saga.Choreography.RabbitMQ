using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Saga.RabbitMQ
{
    public class RabbitConsumer<TCommand, TCommandHandler>
        where TCommand : Command
        where TCommandHandler : CommandHandler<TCommand>
    {
        private readonly IConnection _connection;
        private readonly ChannelPool _channelPool;

        private readonly TCommandHandler _handler;

        public RabbitConsumer(IConnection connection, ChannelPool channelPool, TCommandHandler handler)
        {
            _connection = connection;
            _channelPool = channelPool;
            _handler = handler;
        }

        public void Listen()
        {
            var channel = _channelPool.GetChannel<TCommand>(_connection);

            var queueName = typeof(TCommand).Name;

            channel.QueueDeclare(queueName, exclusive: false);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            AsyncEventingBasicConsumer consumer = new(channel);
            consumer.Received += _handler.Handle;

            channel.BasicConsume(
                queue: queueName, 
                autoAck: true, 
                consumer: consumer
            );
        }
    }
}

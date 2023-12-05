using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Saga.RabbitMQ;

namespace Saga.Infrastructure
{
    public class RabbitPublisher
    {
        private readonly IConnection _connection;
        private readonly ChannelPool _channelPool;

        public RabbitPublisher(IConnection connection, ChannelPool channelPool)
        {
            _connection = connection;
            _channelPool = channelPool;
        }

        public void Publish<T>(T command) where T : Command
        {
            var channel = _channelPool.GetChannel<T>(_connection);

            var queueName = typeof(T).Name;

            channel.QueueDeclare(queueName, exclusive: false);
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: channel.CreateBasicProperties(),
                body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command))
            );
        }
    }
}

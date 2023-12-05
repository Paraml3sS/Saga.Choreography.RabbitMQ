using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Saga.RabbitMQ
{
    public class RabbitMqConnectionFactory
    {
        private readonly string connectionString;

        public RabbitMqConnectionFactory(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("RabbitMqConnectionString");
        }

        public IConnection CreateConnection()
        {
            ConnectionFactory connectionFactory = new();

            connectionFactory.Uri = new Uri(connectionString);

            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.DispatchConsumersAsync = true;

            var connection = connectionFactory.CreateConnection();

            return connection;
        }
    }
}

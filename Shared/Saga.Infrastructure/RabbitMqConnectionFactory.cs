using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

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

            var policy = Policy
                .Handle<SocketException>().Or<BrokerUnreachableException>()
                .WaitAndRetry(5, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));

            var connection = policy.Execute(() => 
                connectionFactory.CreateConnection()
            );

            return connection;
        }
    }
}

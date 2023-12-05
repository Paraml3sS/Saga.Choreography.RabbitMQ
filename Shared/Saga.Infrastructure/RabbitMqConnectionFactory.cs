using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Saga.RabbitMQ
{
    public class RabbitMqConnectionFactory(IConfiguration configuration)
    {
        private readonly string connectionString = configuration.GetConnectionString("RabbitMqConnectionString");

        public IConnection CreateConnection()
        {
            ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));

            var policy = Policy
                .Handle<SocketException>().Or<BrokerUnreachableException>()
                .WaitAndRetry(5, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));


            ConnectionFactory connectionFactory = new();

            connectionFactory.Uri = new Uri(connectionString);

            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.DispatchConsumersAsync = true;

            var connection = policy.Execute(() => 
                connectionFactory.CreateConnection()
            );

            return connection;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Saga.RabbitMQ;

namespace Saga.Infrastructure
{
    public static class RabbitMqDiExtensions
    {
        public static void AddRabbitMq(this IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            RabbitMqConnectionFactory connectionFactory = new(builder.Configuration);

            builder.Services.AddSingleton<RabbitMqConnectionFactory>(connectionFactory);
            builder.Services.AddSingleton<IConnection>(connectionFactory.CreateConnection());
            builder.Services.AddSingleton<ChannelPool>();
        }
    }
}

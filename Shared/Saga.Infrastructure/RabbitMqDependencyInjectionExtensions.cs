using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Saga.RabbitMQ;

namespace Saga.Infrastructure
{
    public static class RabbitMqDependencyInjectionExtensions
    {
        public static IRabbitMqBuilder AddRabbitMq(this IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            RabbitMqConnectionFactory connectionFactory = new(builder.Configuration);

            builder.Services.AddSingleton<RabbitMqConnectionFactory>(connectionFactory);
            builder.Services.AddSingleton<IConnection>(connectionFactory.CreateConnection());
            builder.Services.AddSingleton<ChannelPool>();
            builder.Services.AddTransient<RabbitPublisher>();

            return new RabbitMqBuilder(builder.Services);
        }

        public static IRabbitMqBuilder AddCommandHandler<T, THandler>(this IRabbitMqBuilder builder)
            where T: Command
            where THandler: CommandHandler<T>
        {
            builder.Services.AddTransient<THandler>();
            builder.Services.AddTransient<RabbitConsumer<T, THandler>>();

            builder.Services.Configure<CommandConsumersInfo>(info => info.Consumers.Add(typeof(RabbitConsumer<T, THandler>)));

            return builder;
        }
    }




    public class RabbitMqBuilder(IServiceCollection services) : IRabbitMqBuilder
    {
        public IServiceCollection Services => services;
    }

    public interface IRabbitMqBuilder
    {
        public IServiceCollection Services { get; }
    }
}

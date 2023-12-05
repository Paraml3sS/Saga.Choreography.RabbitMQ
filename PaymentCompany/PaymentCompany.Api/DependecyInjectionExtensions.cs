using Microsoft.Extensions.Options;
using Saga.Infrastructure;
using Saga.Messages;

namespace PaymentCompany.Api
{
    public static class DependecyInjectionExtensions
    {
        public static IRabbitMqBuilder AddCommandHandlers(this IRabbitMqBuilder builder)
        {
            builder.AddCommandHandler<PaymentCommand, PaymentCommandHandler>();

            return builder;
        }

        public static IHost UseCommandHandlers(this IHost app)
        {
            var commandsInfo = app.Services.GetRequiredService<IOptions<CommandConsumersInfo>>();

            commandsInfo.Value.Consumers.ForEach(consumer =>
            {
                using var scope = app.Services.CreateScope();
                (scope.ServiceProvider.GetRequiredService(consumer) as IConsumer)?.Listen();
            });

            return app;
        }
    }
}

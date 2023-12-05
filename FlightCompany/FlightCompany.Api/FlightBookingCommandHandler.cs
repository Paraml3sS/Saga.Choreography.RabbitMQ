using Saga.Infrastructure;
using Saga.Messages;

namespace FlightCompany.Api
{
    public class FlightBookingCommandHandler(ILogger<FlightBookingCommandHandler> logger, RabbitPublisher rabbit) : CommandHandler<FlightBookingCommand>
    {
        protected override Task Handle(FlightBookingCommand command)
        {
            logger.LogInformation("Flight booking from {ClientName} from {From} to {To} accepted.", command.ClientName, command.From, command.To);

            PaymentCommand paymentCommand = new(
                ClientName: command.ClientName,
                Amount: command.Amount
            );

            rabbit.Publish(paymentCommand);

            return Task.CompletedTask;
        }
    }
}
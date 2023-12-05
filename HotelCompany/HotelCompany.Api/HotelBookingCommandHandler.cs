using Saga.Infrastructure;
using Saga.Messages;

namespace HotelCompany.Api
{
    public class HotelBookingCommandHandler(ILogger<HotelBookingCommandHandler> logger, RabbitPublisher rabbit) : CommandHandler<HotelBookingCommand>
    {
        protected override Task Handle(HotelBookingCommand command)
        {
            logger.LogInformation("Hotel booking from {ClientName} accepted.", command.ClientName);

            PaymentCommand paymentCommand = new(
                ClientName: command.ClientName,
                Amount: command.Amount
            );

            rabbit.Publish(paymentCommand);


            return Task.CompletedTask;
        }
    }
}
using Saga.Infrastructure;
using Saga.Messages;

namespace PaymentCompany.Api
{
    public class PaymentCommandHandler(ILogger<PaymentCommandHandler> _logger) : CommandHandler<PaymentCommand>
    {
        protected override Task Handle(PaymentCommand command)
        {
            _logger.LogInformation("Payment from {ClientName} with {Amount}$.", command.ClientName, command.Amount);

            return Task.CompletedTask;
        }
    }
}

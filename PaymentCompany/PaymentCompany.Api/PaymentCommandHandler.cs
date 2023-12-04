using Saga.Messages;
using Saga.RabbitMQ;

namespace PaymentCompany.Api
{
    public class PaymentCommandHandler : CommandHandler<PaymentCommand>
    {
        protected override Task Handle(PaymentCommand command)
        {
            Console.WriteLine($"Payment from {command.ClientName} with {command.Amount}$.");

            return Task.CompletedTask;
        }
    }
}

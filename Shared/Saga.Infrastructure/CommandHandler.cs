using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace Saga.Infrastructure
{
    public abstract class CommandHandler<TCommand>
    {
        public Task Handle(object sender, BasicDeliverEventArgs @event)
        {
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var command = JsonConvert.DeserializeObject<TCommand>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            return Handle(command);
        }

        protected abstract Task Handle(TCommand command);
    }
}

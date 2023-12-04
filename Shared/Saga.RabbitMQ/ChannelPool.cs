using System.Collections.Concurrent;
using RabbitMQ.Client;

namespace Saga.RabbitMQ
{
    public class ChannelPool
    {
        private ConcurrentDictionary<Type, IModel> Channels { get; set; } = new ConcurrentDictionary<Type, IModel>();

        public IModel GetChannel<T>(IConnection connection) where T : Command
            => Channels.GetOrAdd(typeof(T), (type) => connection.CreateModel());
    }
}

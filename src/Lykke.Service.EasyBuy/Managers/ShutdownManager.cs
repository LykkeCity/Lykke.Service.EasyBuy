using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;

namespace Lykke.Service.EasyBuy.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly IEnumerable<OrderBookSubscriber> _orderBookSubscribers;

        public ShutdownManager(
            IEnumerable<OrderBookSubscriber> orderBookSubscribers)
        {
            _orderBookSubscribers = orderBookSubscribers;
        }
        
        public Task StopAsync()
        {
            foreach (var subscriber in _orderBookSubscribers)
            {
                subscriber.Stop();
            }

            return Task.CompletedTask;
        }
    }
}

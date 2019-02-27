using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;

namespace Lykke.Service.EasyBuy.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly IEnumerable<OrderBookSubscriber> _orderBookSubscribers;

        public StartupManager(
            IEnumerable<OrderBookSubscriber> orderBookSubscribers)
        {
            _orderBookSubscribers = orderBookSubscribers;
        }
        
        public Task StartAsync()
        {
            foreach (var subscriber in _orderBookSubscribers)
            {
                subscriber.Start();
            }

            return Task.CompletedTask;
        }
    }
}

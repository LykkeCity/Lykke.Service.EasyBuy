using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.DomainServices.Timers;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;

namespace Lykke.Service.EasyBuy.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly IEnumerable<OrderBookSubscriber> _orderBookSubscribers;
        private readonly OrdersProcessorTimer _ordersProcessorTimer;

        public StartupManager(
            IEnumerable<OrderBookSubscriber> orderBookSubscribers,
            OrdersProcessorTimer ordersProcessorTimer)
        {
            _orderBookSubscribers = orderBookSubscribers;
            _ordersProcessorTimer = ordersProcessorTimer;
        }
        
        public Task StartAsync()
        {
            foreach (var subscriber in _orderBookSubscribers)
            {
                subscriber.Start();
            }
            
            _ordersProcessorTimer.Start();

            return Task.CompletedTask;
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.DomainServices.Timers;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;

namespace Lykke.Service.EasyBuy.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly IEnumerable<OrderBookSubscriber> _orderBookSubscribers;
        private readonly OrdersProcessorTimer _ordersProcessorTimer;

        public ShutdownManager(
            IEnumerable<OrderBookSubscriber> orderBookSubscribers,
            OrdersProcessorTimer ordersProcessorTimer)
        {
            _orderBookSubscribers = orderBookSubscribers;
            _ordersProcessorTimer = ordersProcessorTimer;
        }
        
        public Task StopAsync()
        {
            foreach (var subscriber in _orderBookSubscribers)
            {
                subscriber.Stop();
            }
            
            _ordersProcessorTimer.Stop();

            return Task.CompletedTask;
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.Rabbit.Publishers;
using Lykke.Service.EasyBuy.Timers;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;

namespace Lykke.Service.EasyBuy.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly IEnumerable<OrderBookSubscriber> _orderBookSubscribers;
        private readonly PricesPublisher _pricesPublisher;
        private readonly OrdersTimer _ordersTimer;
        private readonly PricesTimer _pricesTimer;

        public ShutdownManager(
            IEnumerable<OrderBookSubscriber> orderBookSubscribers,
            OrdersTimer ordersTimer,
            PricesTimer pricesTimer,
            PricesPublisher pricesPublisher)
        {
            _orderBookSubscribers = orderBookSubscribers;
            _ordersTimer = ordersTimer;
            _pricesTimer = pricesTimer;
            _pricesPublisher = pricesPublisher;
        }

        public Task StopAsync()
        {
            _pricesTimer.Stop();

            _ordersTimer.Stop();

            _pricesPublisher.Stop();

            foreach (OrderBookSubscriber orderBookSubscriber in _orderBookSubscribers)
                orderBookSubscriber.Stop();

            return Task.CompletedTask;
        }
    }
}

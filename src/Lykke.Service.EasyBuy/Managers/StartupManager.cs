using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.Rabbit.Publishers;
using Lykke.Service.EasyBuy.Timers;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;

namespace Lykke.Service.EasyBuy.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly IEnumerable<OrderBookSubscriber> _orderBookSubscribers;
        private readonly PricesPublisher _pricesPublisher;
        private readonly IPriceService _priceService;
        private readonly OrdersTimer _ordersTimer;
        private readonly PricesTimer _pricesTimer;

        public StartupManager(
            IEnumerable<OrderBookSubscriber> orderBookSubscribers,
            OrdersTimer ordersTimer,
            PricesTimer pricesTimer,
            PricesPublisher pricesPublisher,
            IPriceService priceService)
        {
            _orderBookSubscribers = orderBookSubscribers;
            _ordersTimer = ordersTimer;
            _pricesTimer = pricesTimer;
            _pricesPublisher = pricesPublisher;
            _priceService = priceService;
        }

        public async Task StartAsync()
        {
            await _priceService.InitializeAsync();
            
            foreach (OrderBookSubscriber orderBookSubscriber in _orderBookSubscribers)
                orderBookSubscriber.Start();

            _pricesPublisher.Start();
            
            _ordersTimer.Start();

            _pricesTimer.Start();
        }
    }
}

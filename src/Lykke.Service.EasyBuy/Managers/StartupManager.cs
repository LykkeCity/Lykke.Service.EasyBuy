using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Timers;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;

namespace Lykke.Service.EasyBuy.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly IEnumerable<OrderBookSubscriber> _orderBookSubscribers;
        private readonly IPricesGenerator _pricesGenerator;
        private readonly IPricesPublisher _pricesPublisher;
        private readonly OrdersProcessorTimer _ordersProcessorTimer;

        public StartupManager(
            IEnumerable<OrderBookSubscriber> orderBookSubscribers,
            IPricesGenerator pricesGenerator,
            OrdersProcessorTimer ordersProcessorTimer,
            IPricesPublisher pricesPublisher)
        {
            _orderBookSubscribers = orderBookSubscribers;
            _pricesGenerator = pricesGenerator;
            _ordersProcessorTimer = ordersProcessorTimer;
            _pricesPublisher = pricesPublisher;
        }
        
        public async Task StartAsync()
        {
            foreach (var subscriber in _orderBookSubscribers)
            {
                subscriber.Start();
            }
            
            _ordersProcessorTimer.Start();

            _pricesPublisher.Start();

            await _pricesGenerator.StartAll();
        }
    }
}

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
        private readonly IPricesPublisher _pricesPublisher;
        private readonly OrdersProcessorTimer _ordersProcessorTimer;
        private readonly PricesGeneratorTimer _pricesGeneratorTimer;

        public StartupManager(
            IEnumerable<OrderBookSubscriber> orderBookSubscribers,
            OrdersProcessorTimer ordersProcessorTimer,
            PricesGeneratorTimer pricesGeneratorTimer,
            IPricesPublisher pricesPublisher)
        {
            _orderBookSubscribers = orderBookSubscribers;
            _ordersProcessorTimer = ordersProcessorTimer;
            _pricesGeneratorTimer = pricesGeneratorTimer;
            _pricesPublisher = pricesPublisher;
        }
        
        public Task StartAsync()
        {
            foreach (var subscriber in _orderBookSubscribers)
            {
                subscriber.Start();
            }
            
            _ordersProcessorTimer.Start();

            _pricesGeneratorTimer.Start();

            _pricesPublisher.Start();

            return Task.CompletedTask;
        }
    }
}

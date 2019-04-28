using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.EasyBuy.Domain.Entities.OrderBooks;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Extensions;
using Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Subscribers;
using OrderBook = Lykke.Common.ExchangeAdapter.Contracts.OrderBook;

namespace Lykke.Service.EasyBuy.Rabbit.Subscribers
{
    [UsedImplicitly]
    public class OrderBookSubscriber : IDisposable
    {
        private readonly SubscriberSettings _subscriberSettings;
        private readonly IOrderBookService _orderBookService;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private RabbitMqSubscriber<OrderBook> _subscriber;

        public OrderBookSubscriber(
            SubscriberSettings subscriberSettings,
            IOrderBookService orderBookService,
            ILogFactory logFactory)
        {
            _subscriberSettings = subscriberSettings;
            _orderBookService = orderBookService;
            _logFactory = logFactory;

            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .ForSubscriber(_subscriberSettings.ConnectionString, _subscriberSettings.Exchange,
                    _subscriberSettings.QueueSuffix);

            settings.DeadLetterExchangeName = null;

            _subscriber = new RabbitMqSubscriber<OrderBook>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<OrderBook>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        private async Task ProcessMessageAsync(OrderBook orderBook)
        {
            try
            {
                await _orderBookService.HandleAsync(new Domain.Entities.OrderBooks.OrderBook
                {
                    Exchange = orderBook.Source,
                    AssetPair = orderBook.Asset,
                    Timestamp = orderBook.Timestamp,
                    SellLevels = orderBook.Asks.Select(o => new OrderBookLevel
                    {
                        Price = o.Price,
                        Volume = o.Volume
                    }).ToList(),
                    BuyLevels = orderBook.Bids.Select(o => new OrderBookLevel
                    {
                        Price = o.Price,
                        Volume = o.Volume
                    }).ToList()
                });
            }
            catch (Exception exception)
            {
                _log.ErrorWithDetails(exception, "An error occurred during processing lykke order book", orderBook);
            }
        }
    }
}

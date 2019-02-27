using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.Settings.ServiceSettings;
using OrderBook = Lykke.Common.ExchangeAdapter.Contracts.OrderBook;

namespace Lykke.Service.EasyBuy.Rabbit.Subscribers
{
    [UsedImplicitly]
    public class OrderBookSubscriber : IDisposable
    {
        private readonly string _exchangeName;
        private readonly RabbitSettings _settings;
        private readonly IOrderBookService _orderBookService;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private RabbitMqSubscriber<OrderBook> _subscriber;
        
        public OrderBookSubscriber(
            string exchangeName,
            RabbitSettings settings,
            IOrderBookService orderBookService,
            ILogFactory logFactory)
        {
            _exchangeName = exchangeName;
            _settings = settings;
            _orderBookService = orderBookService;
            _logFactory = logFactory;

            _log = logFactory.CreateLog(this);
        }
        
        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .ForSubscriber(_settings.ConnectionString, _settings.ExchangeName, _settings.QueueSuffix);

            settings.DeadLetterExchangeName = null;
            settings.IsDurable = false;

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
                var ob = Mapper.Map<Domain.OrderBook>(orderBook);
                await _orderBookService.HandleAsync(_exchangeName, ob);
                Console.WriteLine(ob.ToJson());
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing lykke order book", orderBook);
            }
        }
    }
}

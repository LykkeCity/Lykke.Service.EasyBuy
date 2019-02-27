using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class OrderBookService : IOrderBookService
    {
        private readonly object _sync = new object();

        private readonly Dictionary<string, Dictionary<string, OrderBook>> _orderBooks =
            new Dictionary<string, Dictionary<string, OrderBook>>();

        public Task HandleAsync(string exchange, OrderBook orderBook)
        {
            lock (_sync)
            {
                if (!_orderBooks.TryGetValue(exchange, out var orderBooksFromExchange))
                {
                    InitiateExchange(exchange);
                }

                orderBooksFromExchange = _orderBooks[exchange];
                
                if (!orderBooksFromExchange.TryGetValue(orderBook.AssetPair, out var internalOrderBook))
                {
                    InitiateOrderBook(exchange, orderBook.AssetPair);
                }

                internalOrderBook = orderBooksFromExchange[orderBook.AssetPair];

                internalOrderBook.SellLimitOrders.RemoveAll(x => true);
                internalOrderBook.BuyLimitOrders.RemoveAll(x => true);

                internalOrderBook.SellLimitOrders.AddRange(orderBook.SellLimitOrders);
                internalOrderBook.BuyLimitOrders.AddRange(orderBook.BuyLimitOrders);

                internalOrderBook.Timestamp = DateTime.UtcNow;
            }

            return Task.CompletedTask;
        }

        public OrderBook GetByAssetPairId(string exchange, string assetPairId)
        {
            lock (_sync)
            {
                if (!_orderBooks.TryGetValue(exchange, out var orderBooksFromExchange))
                {
                    InitiateExchange(exchange);
                }

                orderBooksFromExchange = _orderBooks[exchange];

                if (!orderBooksFromExchange.TryGetValue(assetPairId, out var internalOrderBook))
                {
                    InitiateOrderBook(exchange, assetPairId);
                }

                return orderBooksFromExchange[assetPairId];
            }
        }
        
        private void InitiateExchange(string exchange)
        {
            _orderBooks[exchange] = new Dictionary<string, OrderBook>();
        }
        
        private void InitiateOrderBook(string exchange, string assetPair)
        {
            var orderBook = new OrderBook()
            {
                AssetPair = assetPair,
                SellLimitOrders = new List<OrderBookLimitOrder>(),
                BuyLimitOrders = new List<OrderBookLimitOrder>()
            };

            _orderBooks[exchange][assetPair] = orderBook;
        }
    }
}

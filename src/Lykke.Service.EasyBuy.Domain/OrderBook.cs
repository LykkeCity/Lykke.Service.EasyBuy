using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.EasyBuy.Domain
{
    public class OrderBook
    {
        public string AssetPair { get; set; }

        public DateTime Timestamp { get; set; }

        public List<OrderBookLimitOrder> SellLimitOrders { get; set; }

        public List<OrderBookLimitOrder> BuyLimitOrders { get; set; }
        
        public OrderBookLimitOrder BestSellOrder()
        {
            return SellLimitOrders
                .OrderBy(x => x.Price)
                .FirstOrDefault();
        }
        
        public OrderBookLimitOrder BestBuyOrder()
        {
            return BuyLimitOrders
                .OrderByDescending(x => x.Price)
                .FirstOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;

namespace Lykke.Service.EasyBuy.Domain.Entities.OrderBooks
{
    /// <summary>
    /// Represents an order book.
    /// </summary>
    public class OrderBook
    {
        /// <summary>
        /// The name of exchange.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// The name of asset pair.
        /// </summary> 
        public string AssetPair { get; set; }

        /// <summary>
        /// The order book creation date.
        /// </summary> 
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// A collection of sell order book levels.
        /// </summary> 
        public IReadOnlyList<OrderBookLevel> SellLevels { get; set; }

        /// <summary>
        /// A collection of buy order book levels.
        /// </summary> 
        public IReadOnlyList<OrderBookLevel> BuyLevels { get; set; }
    }
}

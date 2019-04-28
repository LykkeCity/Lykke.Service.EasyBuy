using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Lykke.Service.EasyBuy.Domain.Entities.OrderBooks;
using Lykke.Service.EasyBuy.Domain.Exceptions;

namespace Lykke.Service.EasyBuy.Domain.Entities.Prices
{
    /// <summary>
    /// Represents a price details.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// The unique identifier of price.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of asset pair.
        /// </summary> 
        public string AssetPair { get; set; }

        /// <summary>
        /// The value of price.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// The maximum allowed base volume.
        /// </summary>
        public decimal BaseVolume { get; set; }

        /// <summary>
        /// The maximum allowed quote volume.
        /// </summary>
        public decimal QuoteVolume { get; set; }

        /// <summary>
        /// The date since the price valid.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// The date until the price is valid.
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// The name of exchange that used as a price source.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// The original price value
        /// </summary>
        public decimal OriginalValue { get; set; }

        /// <summary>
        /// The markup that applied to the original price.
        /// </summary>
        public decimal Markup { get; set; }

        /// <summary>
        /// The date of price creation. 
        /// </summary>
        public DateTime CreatedDate { get; set; }

        public static Price Calculate(OrderBook orderBook, decimal quoteVolume, decimal markup, DateTime validFrom,
            DateTime validTo, int priceAccuracy, int volumeAccuracy)
        {
            if (orderBook == null)
                throw new ArgumentNullException(nameof(orderBook));

            if (quoteVolume <= 0)
                throw new FailedOperationException("Quoting volume required.");

            if (markup < 0 || 1 < markup)
                throw new FailedOperationException("Invalid markup.");

            IReadOnlyList<OrderBookLevel> sellLevels = orderBook.SellLevels;

            if (sellLevels == null || sellLevels.Count == 0)
                throw new FailedOperationException("Empty order book.");

            decimal cumulativeVolume = 0;
            decimal oppositeVolume = 0;

            foreach (OrderBookLevel sellLevel in sellLevels.OrderBy(o => o.Price))
            {
                cumulativeVolume += sellLevel.Volume;
                oppositeVolume += sellLevel.Volume * sellLevel.Price;

                if (quoteVolume <= oppositeVolume)
                    break;
            }

            if (oppositeVolume < quoteVolume)
                throw new FailedOperationException("No liquidity.");

            decimal originalPrice = (oppositeVolume / cumulativeVolume)
                .TruncateDecimalPlaces(priceAccuracy, true);

            decimal price = (originalPrice * (1 + markup))
                .TruncateDecimalPlaces(priceAccuracy, true);

            decimal volume = Math.Round(quoteVolume / price, volumeAccuracy);

            return new Price
            {
                Id = Guid.NewGuid().ToString(),
                AssetPair = orderBook.AssetPair,
                Value = price,
                BaseVolume = volume,
                QuoteVolume = quoteVolume,
                ValidFrom = validFrom,
                ValidTo = validTo,
                Exchange = orderBook.Exchange,
                OriginalValue = originalPrice,
                Markup = markup,
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}

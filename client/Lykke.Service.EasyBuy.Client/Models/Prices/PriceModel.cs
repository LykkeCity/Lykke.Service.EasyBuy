using System;
using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models.Prices
{
    /// <summary>
    /// Represents a price details.
    /// </summary>
    [PublicAPI]
    public class PriceModel
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
    }
}

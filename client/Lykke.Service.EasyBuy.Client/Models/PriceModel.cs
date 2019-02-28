using System;
using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents calculated price with finite lifetime.
    /// </summary>
    [PublicAPI]
    public class PriceModel
    {
        /// <summary>
        /// Price's unique identifier.
        /// </summary>
        public string Id { set; get; }
        
        /// <summary>
        /// Asset pair for which the price is calculated.
        /// </summary>
        public string AssetPair { set; get; }
        
        /// <summary>
        /// Direction in which client wishes to perform an operation.
        /// </summary>
        public OrderType Type { set; get; }
        
        /// <summary>
        /// Possible base volume of the operation.
        /// </summary>
        public decimal BaseVolume { set; get; }
        
        /// <summary>
        /// Desired quoting volume of the operation.
        /// </summary>
        public decimal QuotingVolume { set; get; }
        
        /// <summary>
        /// Markup that was applied to the price.
        /// </summary>
        public decimal Markup { set; get; }
        
        /// <summary>
        /// Total calculated price.
        /// </summary>
        public decimal Price { set; get; }

        /// <summary>
        /// Calculated price.
        /// </summary>
        public decimal OriginalPrice { set; get; }
        
        /// <summary>
        /// Exchange according to which the price was calculated.
        /// </summary>
        public string Exchange { set; get; }
        
        /// <summary>
        /// When the price becomes valid.
        /// </summary>
        public DateTime ValidFrom { set; get; }
        
        /// <summary>
        /// When the price stops being valid.
        /// </summary>
        public DateTime ValidTo { set; get; }
    }
}

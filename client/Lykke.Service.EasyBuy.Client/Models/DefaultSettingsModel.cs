using System;
using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents default price markup and lifetime settings.
    /// </summary>
    [PublicAPI]
    public class DefaultSettingsModel
    {
        /// <summary>
        /// Default markup.
        /// </summary>
        public decimal? Markup { set; get; }
        
        /// <summary>
        /// Default allowed overlap time.
        /// </summary>
        public TimeSpan? OverlapTime { set; get; }
        
        /// <summary>
        /// Default price lifetime.
        /// </summary>
        public TimeSpan? PriceLifetime { set; get; }
        
        /// <summary>
        /// Default recalculation interval.
        /// </summary>
        public TimeSpan? RecalculationInterval { set; get; }
        
        /// <summary>
        /// Default orders processing period.
        /// </summary>
        public TimeSpan? OrdersProcessorPeriod { set; get; }

        /// <summary>
        /// Default price generation period.
        /// </summary>
        public TimeSpan? PriceGeneratorPeriod { set; get; }
    }
}

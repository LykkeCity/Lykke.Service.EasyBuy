using System;
using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents default price markup and lifetime settings.
    /// </summary>
    [PublicAPI]
    public class PriceSettingsModel
    {
        /// <summary>
        /// Default markup.
        /// </summary>
        public decimal Markup { set; get; }
        
        /// <summary>
        /// Default lifetime.
        /// </summary>
        public TimeSpan Lifetime { set; get; }
    }
}

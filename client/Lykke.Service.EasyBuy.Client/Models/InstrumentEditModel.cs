using System;
using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents changes that should be made.
    /// </summary>
    [PublicAPI]
    public class InstrumentEditModel
    {
        /// <summary>
        /// The identifier of an internal asset pair.
        /// </summary>
        public string AssetPair { set; get; }
        
        /// <summary>
        /// Exchange from which to take prices for given asset pair.
        /// </summary>
        public string Exchange { set; get; }
        
        /// <summary>
        /// For how long will calculated price be valid.
        /// </summary>
        public TimeSpan? PriceLifetime { set; get; }
        
        /// <summary>
        /// Added markup. If null, default value from settings is used.
        /// </summary>
        public decimal? Markup { set; get; }
        
        /// <summary>
        /// Current state of the asset pair.
        /// </summary>
        public InstrumentState State { get; set; }
    }
}

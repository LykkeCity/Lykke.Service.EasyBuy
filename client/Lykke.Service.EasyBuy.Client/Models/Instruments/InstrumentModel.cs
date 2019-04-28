using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.EasyBuy.Client.Models.Instruments
{
    /// <summary>
    /// Represents an instrument settings.
    /// </summary>
    [PublicAPI]
    public class InstrumentModel
    {
        /// <summary>
        /// The unique identifier of instrument.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of asset pair.
        /// </summary>
        public string AssetPair { get; set; }

        /// <summary>
        /// The name of exchange that used as a price source.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// The time interval while the price is available to create orders.
        /// </summary>
        public TimeSpan Lifetime { get; set; }

        /// <summary>
        /// The time interval since price live time expired but the price continues to be available.
        /// </summary>
        public TimeSpan OverlapTime { get; set; }

        /// <summary>
        /// The markup that applied to the original price.
        /// </summary>
        public decimal Markup { get; set; }

        /// <summary>
        /// The minimal quote volume which could be used to create order.
        /// </summary>
        public decimal MinQuoteVolume { get; set; }

        /// <summary>
        /// The maximum quote volume which could be used to create order.
        /// </summary>
        public decimal MaxQuoteVolume { get; set; }

        /// <summary>
        /// Indicated the current status of instrument.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public InstrumentStatus Status { get; set; }
    }
}

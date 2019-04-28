using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.EasyBuy.Client.Models.Orders
{
    /// <summary>
    /// Represents an order.
    /// </summary>
    [PublicAPI]
    public class OrderModel
    {
        /// <summary>
        /// The unique identifier of order.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The identifier of client.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The identifier of price.
        /// </summary>
        public string PriceId { get; set; }

        /// <summary>
        /// The name of asset pair.
        /// </summary> 
        public string AssetPair { get; set; }

        /// <summary>
        /// The base asset volume.
        /// </summary>
        public decimal BaseVolume { get; set; }

        /// <summary>
        /// The quote asset volume.
        /// </summary>
        public decimal QuoteVolume { get; set; }

        /// <summary>
        /// The status of order.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status { get; set; }

        /// <summary>
        /// The details of error that occurred while processing order.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The date of order creation.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}

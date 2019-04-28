using System;

namespace Lykke.Service.EasyBuy.Domain.Entities.Orders
{
    /// <summary>
    /// Represents an order.
    /// </summary>
    public class Order
    {
        public Order()
        {
        }

        public Order(string clientId, string priceId, string assetPair, decimal baseVolume, decimal quoteVolume)
        {
            Id = Guid.NewGuid().ToString();
            ClientId = clientId;
            PriceId = priceId;
            AssetPair = assetPair;
            BaseVolume = baseVolume;
            QuoteVolume = quoteVolume;
            Status = OrderStatus.New;
            CreatedDate = DateTime.UtcNow;
        }

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
        public OrderStatus Status { get; set; }

        /// <summary>
        /// The details of error that occurred while processing order.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The date of order creation.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        public void Cancel(string error)
        {
            Status = OrderStatus.Cancelled;
            Error = error;
        }

        public void Reserved()
        {
            Status = OrderStatus.Reserved;
        }

        public void Complete()
        {
            Status = OrderStatus.Completed;
        }
    }
}

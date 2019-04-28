using System;

namespace Lykke.Service.EasyBuy.Domain.Entities.Orders
{
    /// <summary>
    /// Represent a transfer.
    /// </summary>
    public class Transfer
    {
        public Transfer()
        {
        }

        public Transfer(string orderId, TransferType type)
        {
            Id = Guid.NewGuid().ToString();
            OrderId = orderId;
            Type = type;
            CreatedDate = DateTime.UtcNow;
        }
        
        /// <summary>
        /// The unique identifier of transfer.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The identifier of order.
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// The type of transfer.
        /// </summary>
        public TransferType Type { get; set; }

        /// <summary>
        /// The date of transfer creation.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}

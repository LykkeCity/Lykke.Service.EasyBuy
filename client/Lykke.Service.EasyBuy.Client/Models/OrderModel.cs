using System;
using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents user's order.
    /// </summary>
    [PublicAPI]
    public class OrderModel
    {
        /// <summary>
        /// Order's unique identifier.
        /// </summary>
        public string Id { set; get; }
        
        /// <summary>
        /// Client's wallet Id.
        /// </summary>
        public string WalletId { set; get; }
        
        /// <summary>
        /// Asset pair of the order.
        /// </summary>
        public string AssetPair { set; get; }
        
        /// <summary>
        /// Type of the order.
        /// </summary>
        public OrderType Type { set; get; }
        
        /// <summary>
        /// Id of the calculated price snapshot.
        /// </summary>
        public string PriceId { set; get; }
        
        /// <summary>
        /// Desired volume of the order.
        /// </summary>
        public decimal Volume { set; get; }
        
        /// <summary>
        /// Date and time of order creation.
        /// </summary>
        public DateTime CreatedTime { set; get; }
        
        /// <summary>
        /// 
        /// </summary>
        public OrderStatusModel Status { set; get; }
    }
}

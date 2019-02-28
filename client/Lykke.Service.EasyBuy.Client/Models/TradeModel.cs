using System;
using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents executed trade.
    /// </summary>
    [PublicAPI]
    public class TradeModel
    {
        /// <summary>
        /// Trade's unique identifier.
        /// </summary>
        public string Id { set; get; }
        
        /// <summary>
        /// Id of user's wallet.
        /// </summary>
        public string WalletId { set; get; }
        
        /// <summary>
        /// Id of the order.
        /// </summary>
        public string OrderId { set; get; }
        
        /// <summary>
        /// Order type.
        /// </summary>
        public OrderType Type { set; get; }
        
        /// <summary>
        /// Base volume.
        /// </summary>
        public decimal BaseVolume { set; get; }
        
        /// <summary>
        /// Quoting volume.
        /// </summary>
        public decimal QuotingVolume { set; get; }
        
        /// <summary>
        /// Date and time when trade was performed.
        /// </summary>
        public DateTime DateTime { set; get; }
    }
}

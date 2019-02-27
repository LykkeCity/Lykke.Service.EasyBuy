using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents request to calculate price.
    /// </summary>
    [PublicAPI]
    public class CalculatePriceModel
    {
        /// <summary>
        /// Client's wallet Id.
        /// </summary>
        public string WalletId { set; get; }
        
        /// <summary>
        /// Asset pair for which the price should be calculated.
        /// </summary>
        public string AssetPair { set; get; }
        
        /// <summary>
        /// Direction in which client wishes to perform an operation.
        /// </summary>
        public OrderType Type { set; get; }
        
        /// <summary>
        /// Desired volume of the operation.
        /// </summary>
        public decimal QuotingVolume { set; get; }
    }
}

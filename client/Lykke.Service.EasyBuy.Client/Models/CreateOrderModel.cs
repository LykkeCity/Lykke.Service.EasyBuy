using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public class CreateOrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string WalletId;
        
        /// <summary>
        /// 
        /// </summary>
        public string PriceId;
        
        /// <summary>
        /// 
        /// </summary>
        public decimal QuotingVolume;
    }
}

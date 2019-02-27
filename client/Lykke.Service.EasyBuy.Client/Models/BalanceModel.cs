using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Represents balance item.
    /// </summary>
    [PublicAPI]
    public class BalanceModel
    {
        /// <summary>
        /// Id of the asset.
        /// </summary>
        public string AssetId { set; get; }
        
        /// <summary>
        /// Available amount.
        /// </summary>
        public decimal Available { set; get; }
        
        /// <summary>
        /// Reserved amount.
        /// </summary>
        public decimal Reserved { set; get; }
    }
}

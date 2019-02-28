using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// Direction of the order.
    /// </summary>
    [PublicAPI]
    public enum OrderType
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None,
        
        /// <summary>
        /// Buy direction.
        /// </summary>
        Buy,
        
        /// <summary>
        /// Sell direction.
        /// </summary>
        Sell
    }
}

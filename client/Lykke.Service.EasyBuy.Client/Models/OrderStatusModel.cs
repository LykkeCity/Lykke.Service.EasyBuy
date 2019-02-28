using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public enum OrderStatusModel
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        
        /// <summary>
        /// 
        /// </summary>
        New,
        
        /// <summary>
        /// 
        /// </summary>
        Cancelled,
        
        /// <summary>
        /// 
        /// </summary>
        Reserved,
        
        /// <summary>
        /// 
        /// </summary>
        Completed
    }
}

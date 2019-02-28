using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Api;

namespace Lykke.Service.EasyBuy.Client
{
    /// <summary>
    /// Easy buy service client interface.
    /// </summary>
    [PublicAPI]
    public interface IEasyBuyClient
    {
        /// <summary>
        /// 
        /// </summary>
        IBalancesApi BalancesApi { get; }
        
        /// <summary>
        /// 
        /// </summary>
        IInstrumentsApi InstrumentsApi { get; }
        
        /// <summary>
        /// 
        /// </summary>
        IOrdersApi OrdersApi { get; }
        
        /// <summary>
        /// 
        /// </summary>
        IPricesApi PricesApi { get; }
        
        /// <summary>
        /// 
        /// </summary>
        ISettingsApi SettingsApi { get; }
        
        /// <summary>
        /// 
        /// </summary>
        ITradesApi TradesApi { get; }
    }
}

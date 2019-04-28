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
        /// Provides methods to request balance.
        /// </summary>
        IBalancesApi BalancesApi { get; }

        /// <summary>
        /// Provides methods to work with instruments.
        /// </summary>
        IInstrumentsApi InstrumentsApi { get; }

        /// <summary>
        /// Provides methods to work with orders.
        /// </summary>
        IOrdersApi OrdersApi { get; }

        /// <summary>
        /// Provides method to work with prices.
        /// </summary>
        IPricesApi PricesApi { get; }

        /// <summary>
        /// Provides methods to work with EasyBuy settings.
        /// </summary>
        ISettingsApi SettingsApi { get; }

        /// <summary>
        /// Provides methods to work with exchanges.
        /// </summary>
        IExchangesApi ExchangesApi { get; }
    }
}

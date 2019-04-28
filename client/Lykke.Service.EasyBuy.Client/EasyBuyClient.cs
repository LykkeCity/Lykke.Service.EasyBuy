using Lykke.HttpClientGenerator;
using Lykke.Service.EasyBuy.Client.Api;

namespace Lykke.Service.EasyBuy.Client
{
    /// <inheritdoc/>
    public class EasyBuyClient : IEasyBuyClient
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EasyBuyClient"/> with <param name="httpClientGenerator"></param>.
        /// </summary> 
        public EasyBuyClient(IHttpClientGenerator httpClientGenerator)
        {
            BalancesApi = httpClientGenerator.Generate<IBalancesApi>();
            InstrumentsApi = httpClientGenerator.Generate<IInstrumentsApi>();
            OrdersApi = httpClientGenerator.Generate<IOrdersApi>();
            PricesApi = httpClientGenerator.Generate<IPricesApi>();
            SettingsApi = httpClientGenerator.Generate<ISettingsApi>();
            ExchangesApi = httpClientGenerator.Generate<IExchangesApi>();
        }

        /// <inheritdoc />
        public IBalancesApi BalancesApi { get; }

        /// <inheritdoc />
        public IInstrumentsApi InstrumentsApi { get; }

        /// <inheritdoc />
        public IOrdersApi OrdersApi { get; }

        /// <inheritdoc />
        public IPricesApi PricesApi { get; }

        /// <inheritdoc />
        public ISettingsApi SettingsApi { get; }

        /// <inheritdoc />
        public IExchangesApi ExchangesApi { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client.Models.v3;
using Lykke.Service.Assets.Client.ReadModels;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Entities.OrderBooks;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Cache;
using Lykke.Service.EasyBuy.DomainServices.Extensions;

namespace Lykke.Service.EasyBuy.DomainServices
{
    public class PriceService : IPriceService
    {
        private readonly IPriceRepository _priceRepository;
        private readonly IOrderBookService _orderBookService;
        private readonly IInstrumentService _instrumentService;
        private readonly ISettingsService _settingsService;
        private readonly IAssetsReadModelRepository _assetsRepository;
        private readonly IAssetPairsReadModelRepository _assetPairsRepository;
        private readonly IPricesPublisher _pricesPublisher;
        private readonly ILog _log;

        private readonly InMemoryCache<Price> _cache;

        public PriceService(
            IPriceRepository priceRepository,
            IOrderBookService orderBookService,
            IInstrumentService instrumentService,
            ISettingsService settingsService,
            IAssetsReadModelRepository assetsRepository,
            IAssetPairsReadModelRepository assetPairsRepository,
            IPricesPublisher pricesPublisher,
            ILogFactory logFactory)
        {
            _priceRepository = priceRepository;
            _orderBookService = orderBookService;
            _instrumentService = instrumentService;
            _settingsService = settingsService;
            _assetsRepository = assetsRepository;
            _assetPairsRepository = assetPairsRepository;
            _pricesPublisher = pricesPublisher;
            _log = logFactory.CreateLog(this);

            _cache = new InMemoryCache<Price>(GetKey, false);
        }

        public async Task EnsureAsync(string assetPair, DateTime currentDate)
        {
            Instrument instrument = await _instrumentService.GetByAssetPairAsync(assetPair);

            if (instrument == null)
                throw new FailedOperationException("Unknown instrument.");

            if (instrument.Status != InstrumentStatus.Active)
                return;

            Price currentPrice = await GetByAssetPair(assetPair);

            TimeSpan recalculationInterval = _settingsService.GetRecalculationInterval();

            DateTime currentPriceValidTo = currentPrice?.ValidTo ?? currentDate;

            bool currentPriceNotExpired = currentDate.Add(recalculationInterval) < currentPriceValidTo;

            if (currentPriceNotExpired)
                return;

            TimeSpan expiredDuration = (currentDate - currentPriceValidTo).Duration();

            bool expiredInRecalculationInterval = expiredDuration <= recalculationInterval;

            if (!expiredInRecalculationInterval)
            {
                _log.WarningWithDetails("Price recalculation interval exceeded.", new
                {
                    instrument.AssetPair,
                    CurrentPriceValidTo = currentPriceValidTo,
                    CurrentDate = currentDate,
                    ExpiredDuration = expiredDuration
                });
            }

            DateTime validFrom = expiredInRecalculationInterval ? currentPriceValidTo : currentDate;

            DateTime validTo = validFrom.Add(instrument.Lifetime);

            OrderBook orderBook = _orderBookService.GetByAssetPair(instrument.Exchange, instrument.AssetPair);

            if (orderBook == null)
                return;

            AssetPair assetPairSettings = _assetPairsRepository.TryGet(instrument.AssetPair);

            Asset baseAssetSettings = _assetsRepository.TryGet(assetPairSettings.BaseAssetId);

            Price price = Price.Calculate(orderBook, instrument.MaxQuoteVolume, instrument.Markup, validFrom, validTo,
                assetPairSettings.Accuracy, baseAssetSettings.Accuracy);

            _log.InfoWithDetails("Price calculated.", price);

            await _priceRepository.InsertAsync(price);

            _cache.Set(price);

            await _pricesPublisher.PublishAsync(price);
        }

        public async Task<Price> GetByIdAsync(string priceId)
        {
            return await _priceRepository.GetByIdAsync(priceId);
        }

        public async Task<Price> GetByAssetPair(string assetPair)
        {
            if (!_cache.Initialized)
                await InitializeAsync();

            return _cache.Get(GetKey(assetPair));
        }

        public async Task<IReadOnlyList<Price>> GetActiveAsync()
        {
            IReadOnlyList<Price> prices = await _priceRepository.GetLatestAsync();

            return prices.Where(o => o.ValidTo > DateTime.UtcNow).ToList();
        }

        public async Task InitializeAsync()
        {
            IReadOnlyList<Price> prices = await _priceRepository.GetLatestAsync();

            _cache.Initialize(prices);
        }

        private static string GetKey(Price price)
            => GetKey(price.AssetPair);

        private static string GetKey(string assetPair)
            => assetPair;
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class PricesGenerator : IPricesGenerator
    {
        private readonly IInstrumentsAccessService _instrumentsAccessService;
        private readonly IPricesService _pricesService;
        private readonly IPricesPublisher _pricesPublisher;
        private readonly ISettingsService _settingsService;
        private readonly ILog _log;

        public PricesGenerator(
            IInstrumentsAccessService instrumentsAccessService,
            IPricesPublisher pricesPublisher,
            IPricesService pricesService,
            ISettingsService settingsService,
            ILogFactory logFactory)
        {
            _instrumentsAccessService = instrumentsAccessService;
            _pricesPublisher = pricesPublisher;
            _pricesService = pricesService;
            _settingsService = settingsService;
            _log = logFactory.CreateLog(this);
        }

        public async Task TryExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var defaultSettings = await _settingsService.GetDefaultSettingsAsync();

                var activeInstruments = await _instrumentsAccessService.GetActiveAsync();

                var activePrices = await _pricesService.GetActiveAsync(OrderType.Buy);

                foreach (var instrument in activeInstruments)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    Price price = activePrices.FirstOrDefault(x => x.AssetPair == instrument.AssetPair);

                    TimeSpan recalculationInterval = instrument.RecalculationInterval ?? defaultSettings.RecalculationInterval;

                    if (price == null || price.ValidTo - DateTime.UtcNow < recalculationInterval)
                    {
                        Task.Run(async () => { await TryCalculateAndPublishAsync(instrument, price); })
                            .ContinueWith(t =>
                            {
                                if (t.IsFaulted)
                                    _log.Error(t.Exception, "Something went wrong in calculation and publishing.");
                            });
                    }
                }

            }
            catch (Exception exception)
            {
                _log.Warning(exception.Message, exception);
            }
        }

        private async Task TryCalculateAndPublishAsync(Instrument instrument, Price price)
        {
            var defaultSettings = await _settingsService.GetDefaultSettingsAsync();

            var priceLifetime = instrument.PriceLifetime ?? defaultSettings.PriceLifetime;

            var isTooLong = price.ValidTo - DateTime.UtcNow < -priceLifetime;

            var validFrom = isTooLong ? DateTime.UtcNow : price.ValidTo;

            PricesPack nextPack = await TryToCalculateNext(instrument.AssetPair, instrument.Volume, validFrom);

            TryToPublish(instrument.AssetPair, nextPack);
        }

        private async Task<PricesPack> TryToCalculateNext(string assetPair, decimal volume, DateTime validFrom)
        {
            try
            {
                var context = new { assetPair, volume, validFrom };

                _log.Info("Calculating next pack.", context);

                var result = new PricesPack
                {
                    Buy = await _pricesService.CreateAsync(assetPair, OrderType.Buy, volume, validFrom),

                    Sell = await _pricesService.CreateAsync(assetPair, OrderType.Sell, volume, validFrom)
                };

                _log.Info("Calculating next pack - finished.", context);

                return result;
            }
            catch (Exception e)
            {
                _log.Warning("Exception while trying to calculate next price.", e, assetPair);

                return null;
            }
        }

        private void TryToPublish(string assetPair, PricesPack pack)
        {
            try
            {
                if (pack != null)
                {
                    _pricesPublisher.Publish(pack.Sell);
                }
                else
                {
                    _log.Info("Skipping publishing.", assetPair);
                }
            }
            catch (Exception e)
            {
                _log.Warning("Exception while trying to publish.", e, assetPair);
            }
        }
    }
}

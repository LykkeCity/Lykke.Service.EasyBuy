using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Exceptions;
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

        private readonly SemaphoreSlim _lock;
        private readonly ConcurrentDictionary<string, Task> _tasks;
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _cancelationTokens;

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

            _cancelationTokens = new ConcurrentDictionary<string, CancellationTokenSource>();
            _tasks = new ConcurrentDictionary<string, Task>();
            _lock = new SemaphoreSlim(1, 1);
        }

        public async Task StartAll()
        {
            foreach (var activeInstrument in (await _instrumentsAccessService.GetAllAsync()).Where(x =>
                x.State == InstrumentState.Active))
            {
                await Start(activeInstrument.AssetPair);
            }
        }

        public async Task StopAll()
        {
            foreach (var assetPair in _tasks.Keys)
            {
                await Stop(assetPair);
            }
        }

        public async Task Start(string assetPair)
        {
            try
            {
                await _lock.WaitAsync();

                _log.Info(nameof(Start), "Starting publishing.", assetPair);

                var instrument = (await _instrumentsAccessService.GetAllAsync())
                    .SingleOrDefault(x => x.AssetPair == assetPair);

                if (instrument == null || instrument.State != InstrumentState.Active)
                    throw new FailedOperationException($"No active instrument {assetPair} was found.");

                if (_cancelationTokens.ContainsKey(assetPair))
                    throw new FailedOperationException($"Instrument {assetPair} already running.");

                _cancelationTokens[assetPair] = new CancellationTokenSource();

                var cycleTask = Task.Run(async () => { await HandleGenerationCycleAsync(instrument.AssetPair); })
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                            _log.Error(t.Exception, "Something went wrong in calculation and publishing thread.");
                    });

                _tasks[assetPair] = cycleTask;

                _log.Info(nameof(Start), "Started.", assetPair);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task Stop(string assetPair)
        {
            try
            {
                await _lock.WaitAsync();

                _log.Info(nameof(Stop), "Stopping publishing.", assetPair);

                if (!_tasks.ContainsKey(assetPair) || !_cancelationTokens.ContainsKey(assetPair))
                    throw new FailedOperationException($"No instrument {assetPair} was found running.");

                _cancelationTokens[assetPair].Cancel();

                await _tasks[assetPair];

                _cancelationTokens.TryRemove(assetPair, out _);
                _tasks.TryRemove(assetPair, out _);

                _log.Info(nameof(Stop), "Stopped.", assetPair);
            }
            finally
            {
                _lock.Release();
            }
        }


        private async Task HandleGenerationCycleAsync(string assetPair)
        {
            var calculationTime = DateTime.UtcNow;

            while (!_cancelationTokens.ContainsKey(assetPair) &&
                   !_cancelationTokens[assetPair].IsCancellationRequested)
            {
                try
                {
                    var defaultSettings = await _settingsService.GetDefaultSettingsAsync();


                    var instrument = await _instrumentsAccessService.GetByAssetPairIdAsync(assetPair);

                    var nextPack = await TryToCalculateNext(instrument.AssetPair, instrument.Volume, calculationTime);

                    TryToPublish(instrument.AssetPair, nextPack);
                    

                    var recalculationInterval = instrument.RecalculationInterval ?? defaultSettings.RecalculationInterval;

                    var priceLifetime = instrument.PriceLifetime ?? defaultSettings.PriceLifetime;


                    DateTime whenToStartNextTime = calculationTime + priceLifetime - recalculationInterval;

                    TimeSpan delayUntilNextTime = whenToStartNextTime - DateTime.UtcNow;

                    await Task.Delay(delayUntilNextTime, _cancelationTokens[instrument.AssetPair].Token);


                    calculationTime += priceLifetime;
                }
                catch (TaskCanceledException)
                {
                    _log.Info($"Price generation was stopped.", assetPair);
                }
                catch (Exception e)
                {
                    _log.Error(e, e.Message, assetPair);
                }
            }
        }

        private void TryToPublish(string assetPair, PricesPack pack)
        {
            try
            {
                if (pack != null)
                {
                    _pricesPublisher.Publish(pack.Sell);
                    //_pricesPublisher.Publish(pack.Buy);
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

        private static TimeSpan Min(TimeSpan first, TimeSpan second)
        {
            return first < second ? first : second;
        }

        private static TimeSpan Max(TimeSpan first, TimeSpan second)
        {
            return first > second ? first : second;
        }
    }
}

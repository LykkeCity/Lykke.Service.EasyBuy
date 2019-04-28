using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.Timers
{
    public class PricesTimer : Timer
    {
        private readonly IPriceService _priceService;
        private readonly ISettingsService _settingsService;
        private readonly IInstrumentService _instrumentService;

        public PricesTimer(
            IPriceService priceService,
            ISettingsService settingsService,
            IInstrumentService instrumentService,
            ILogFactory logFactory)
        {
            _priceService = priceService;
            _settingsService = settingsService;
            _instrumentService = instrumentService;

            Log = logFactory.CreateLog(this);
        }

        protected override async Task OnExecuteAsync(CancellationToken cancellation)
        {
            IReadOnlyList<Instrument> instruments = await _instrumentService.GetActiveAsync();

            DateTime currentDate = DateTime.UtcNow;

            IEnumerable<Task> tasks = instruments
                .Select(o => Task
                    .Run(async () => { await _priceService.EnsureAsync(o.AssetPair, currentDate); }, cancellation)
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                            Log.Error(task.Exception, "Something went wrong in calculation and publishing.");
                    }, cancellation));

            await Task.WhenAll(tasks);
        }

        protected override Task<TimeSpan> GetDelayAsync()
        {
            TimeSpan recalculationInterval = _settingsService.GetRecalculationInterval();

            return Task.FromResult(recalculationInterval);
        }
    }
}

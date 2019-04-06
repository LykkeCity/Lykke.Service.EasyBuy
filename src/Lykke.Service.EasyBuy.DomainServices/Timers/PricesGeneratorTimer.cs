using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices.Timers
{
    [UsedImplicitly]
    public class PricesGeneratorTimer : Timer
    {
        private readonly ISettingsService _settingsService;
        private readonly IPricesGenerator _pricesGenerator;
        
        public PricesGeneratorTimer(
            ISettingsService settingsService,
            IPricesGenerator pricesGenerator,
            ILogFactory logFactory)
        {
            _settingsService = settingsService;
            _pricesGenerator = pricesGenerator;

            Log = logFactory.CreateLog(this);
        }
        
        protected override Task OnExecuteAsync(CancellationToken cancellation)
        {
            return _pricesGenerator.TryExecuteAsync(cancellation);
        }

        protected override async Task<TimeSpan> GetDelayAsync()
        {
            var defaultSettings = await _settingsService.GetDefaultSettingsAsync();

            return defaultSettings.PriceGeneratorPeriod;
        }
    }
}

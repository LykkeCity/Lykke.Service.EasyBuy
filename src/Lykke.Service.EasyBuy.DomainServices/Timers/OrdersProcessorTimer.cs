using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices.Timers
{
    [UsedImplicitly]
    public class OrdersProcessorTimer : Timer
    {
        private readonly ISettingsService _settingsService;
        private readonly IOrdersService _ordersService;
        
        public OrdersProcessorTimer(
            ISettingsService settingsService,
            IOrdersService ordersService,
            ILog log)
            : base(log)
        {
            _settingsService = settingsService;
            _ordersService = ordersService;
        }
        
        protected override Task OnExecuteAsync(CancellationToken cancellation)
        {
            return _ordersService.ProcessPendingAsync();
        }

        protected override Task<TimeSpan> GetDelayAsync()
        {
            return _settingsService.GetTimerPeriodAsync();
        }
    }
}

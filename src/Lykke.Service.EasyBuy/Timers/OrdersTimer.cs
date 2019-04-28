using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.Timers
{
    [UsedImplicitly]
    public class OrdersTimer : Timer
    {
        private readonly ISettingsService _settingsService;
        private readonly IOrderService _orderService;

        public OrdersTimer(
            ISettingsService settingsService,
            IOrderService orderService,
            ILogFactory logFactory)
        {
            _settingsService = settingsService;
            _orderService = orderService;

            Log = logFactory.CreateLog(this);
        }

        protected override Task OnExecuteAsync(CancellationToken cancellation)
        {
            return _orderService.ExecuteAsync();
        }

        protected override Task<TimeSpan> GetDelayAsync()
        {
            TimeSpan orderExecutionInterval = _settingsService.GetOrderExecutionInterval();

            return Task.FromResult(orderExecutionInterval);
        }
    }
}

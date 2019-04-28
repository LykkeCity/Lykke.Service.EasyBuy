using System;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class SettingsService : ISettingsService
    {
        private readonly string _instanceName;
        private readonly string _walletId;
        private readonly TimeSpan _recalculationInterval;
        private readonly TimeSpan _orderExecutionInterval;

        public SettingsService(
            string instanceName,
            string walletId,
            TimeSpan recalculationInterval,
            TimeSpan orderExecutionInterval)
        {
            _instanceName = instanceName;
            _walletId = walletId;
            _recalculationInterval = recalculationInterval;
            _orderExecutionInterval = orderExecutionInterval;
        }

        public string GetInstanceName()
            => _instanceName;

        public string GetWalletId()
            => _walletId;

        public TimeSpan GetRecalculationInterval()
            => _recalculationInterval;

        public TimeSpan GetOrderExecutionInterval()
            => _orderExecutionInterval;
    }
}

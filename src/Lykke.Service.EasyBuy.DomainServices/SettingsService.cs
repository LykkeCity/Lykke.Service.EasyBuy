using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class SettingsService : ISettingsService
    {
        private readonly string _instanceName;
        private readonly string _walletId;

        private DefaultSetting _defaultSettings;

        private readonly IDefaultSettingsRepository _settingsRepository;
        
        public SettingsService(
            string instanceName,
            string walletId,
            IDefaultSettingsRepository settingsRepository)
        {
            _instanceName = instanceName;
            _walletId = walletId;
            _defaultSettings = null;
            _settingsRepository = settingsRepository;
        }
        
        public Task<string> GetServiceInstanceNameAsync()
        {
            return Task.FromResult(_instanceName);
        }

        public Task<string> GetWalletIdAsync()
        {
            return Task.FromResult(_walletId);
        }

        public async Task<DefaultSetting> GetDefaultSettingsAsync()
        {
            if (_defaultSettings == null)
            {
                var defaultSettings = await _settingsRepository.GetAsync();

                _defaultSettings = new DefaultSetting
                {
                    Markup = defaultSettings?.Markup ?? 0.01m,
                    OverlapTime = defaultSettings?.OverlapTime ?? TimeSpan.Zero,
                    PriceLifetime = defaultSettings?.PriceLifetime ?? TimeSpan.FromSeconds(20),
                    RecalculationInterval = defaultSettings?.RecalculationInterval ?? TimeSpan.Zero,
                    OrdersProcessorPeriod = defaultSettings?.OrdersProcessorPeriod ?? TimeSpan.FromSeconds(5),
                    PriceGeneratorPeriod = defaultSettings?.PriceGeneratorPeriod ?? TimeSpan.FromMilliseconds(500)
                };
            }

            if (_defaultSettings.PriceGeneratorPeriod == TimeSpan.Zero)
                _defaultSettings.PriceGeneratorPeriod = TimeSpan.FromMilliseconds(500);

            return _defaultSettings;
        }

        public async Task UpdateDefaultSettingsAsync(DefaultSetting defaultSetting)
        {
            _defaultSettings = defaultSetting;

            await _settingsRepository.CreateOrUpdateAsync(defaultSetting);
        }
    }
}

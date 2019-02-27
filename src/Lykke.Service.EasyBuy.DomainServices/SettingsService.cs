using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class SettingsService : ISettingsService
    {
        private readonly string _instanceName;
        private readonly string _clientId;
        private readonly decimal _defaultMarkup;
        private readonly TimeSpan _defaultPriceLifetime;
        private readonly TimeSpan _timerPeriod;
        private readonly IEnumerable<string> _externalExchanges;
        
        public SettingsService(
            string instanceName,
            string clientId,
            decimal defaultMarkup,
            TimeSpan timerPeriod,
            TimeSpan defaultPriceLifetime,
            IEnumerable<string> externalExchanges)
        {
            _instanceName = instanceName;
            _clientId = clientId;
            _defaultMarkup = defaultMarkup;
            _timerPeriod = timerPeriod;
            _defaultPriceLifetime = defaultPriceLifetime;
            _externalExchanges = externalExchanges;
        }
        
        public Task<string> GetServiceInstanceNameAsync()
        {
            return Task.FromResult(_instanceName);
        }

        public Task<string> GetClientIdAsync()
        {
            return Task.FromResult(_clientId);
        }

        public Task<decimal> GetDefaultMarkupAsync()
        {
            return Task.FromResult(_defaultMarkup);
        }

        public Task<TimeSpan> GetDefaultPriceLifetimeAsync()
        {
            return Task.FromResult(_defaultPriceLifetime);
        }

        public Task<TimeSpan> GetTimerPeriodAsync()
        {
            return Task.FromResult(_timerPeriod);
        }

        public Task<IEnumerable<string>> GetExternalExchangesAsync()
        {
            return Task.FromResult(_externalExchanges);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface ISettingsService
    {
        Task<string> GetServiceInstanceNameAsync();
        Task<string> GetClientIdAsync();
        Task<decimal> GetDefaultMarkupAsync();
        Task<TimeSpan> GetDefaultPriceLifetimeAsync();
        Task<TimeSpan> GetTimerPeriodAsync();
        Task<IEnumerable<string>> GetExternalExchangesAsync();
    }
}

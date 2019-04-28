using System;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface ISettingsService
    {
        string GetInstanceName();

        string GetWalletId();

        TimeSpan GetRecalculationInterval();

        TimeSpan GetOrderExecutionInterval();
    }
}

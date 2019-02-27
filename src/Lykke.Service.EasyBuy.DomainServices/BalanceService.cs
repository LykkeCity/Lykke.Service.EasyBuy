using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Balances.AutorestClient.Models;
using Lykke.Service.Balances.Client;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class BalanceService : IBalancesService
    {
        private readonly IBalancesClient _balancesClient;
        private readonly ISettingsService _settingsService;

        public BalanceService(
            IBalancesClient balancesClient,
            ISettingsService settingsService)
        {
            _balancesClient = balancesClient;
            _settingsService = settingsService;
        }

        public async Task<IReadOnlyList<Balance>> GetAsync()
        {
            var balance = await _balancesClient.GetClientBalances(await _settingsService.GetClientIdAsync());

            return balance.Select(x => new Balance
            {
                AssetId = x.AssetId,
                Available = x.Balance,
                Reserved = x.Reserved
            }).ToArray();
        }
    }
}

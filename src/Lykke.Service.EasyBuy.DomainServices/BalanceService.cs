using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Balances.AutorestClient.Models;
using Lykke.Service.Balances.Client;
using Lykke.Service.Balances.Client.ResponseModels;
using Lykke.Service.EasyBuy.Domain.Entities.Balances;
using Lykke.Service.EasyBuy.Domain.Exceptions;
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

        public async Task<IReadOnlyList<Balance>> GetAllAsync()
        {
            try
            {
                string walletId = _settingsService.GetWalletId();

                IEnumerable<ClientBalanceResponseModel> balance = await _balancesClient.GetClientBalances(walletId);

                return balance.Select(o => new Balance(o.AssetId, o.Balance, o.Reserved)).ToList();
            }
            catch (Exception exception)
            {
                throw new FailedOperationException("An error occurred while retrieving balances.", exception);
            }
        }

        public async Task<Balance> GetByAssetAsync(string asset)
        {
            try
            {
                string walletId = _settingsService.GetWalletId();

                ClientBalanceModel balance =
                    await _balancesClient.GetClientBalanceByAssetId(new ClientBalanceByAssetIdModel(asset, walletId));

                return balance == null
                    ? new Balance(asset)
                    : new Balance(asset, balance.Balance, balance.Reserved);
            }
            catch (Exception exception)
            {
                throw new FailedOperationException("An error occurred while retrieving balance.", exception);
            }
        }
    }
}

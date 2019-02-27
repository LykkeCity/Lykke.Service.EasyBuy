using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ClientAccount.Client.AutorestClient.Models;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.ExchangeOperations.Client;
using Lykke.Service.ExchangeOperations.Client.Models;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class InternalTransfersService : IInternalTransfersService
    {
        private readonly IExchangeOperationsServiceClient _exchangeOperationsServiceClient;
        private readonly IClientAccountClient _clientAccountClient;
        
        private readonly ConcurrentDictionary<string, string> _walletIdsMapping = new ConcurrentDictionary<string, string>();

        public InternalTransfersService(
            IExchangeOperationsServiceClient exchangeOperationsServiceClient,
            IClientAccountClient clientAccountClient)
        {
            _exchangeOperationsServiceClient = exchangeOperationsServiceClient;
            _clientAccountClient = clientAccountClient;
        }
        
        public async Task TransferAsync(
            string transferId,
            string easyBuyClientId,
            string userWalletId,
            bool direct,
            string assetId,
            decimal amount)
        {
            
            var result = await _exchangeOperationsServiceClient.ExchangeOperations.TransferAsync(
                new TransferRequestModel
                {
                    SourceClientId = direct ? easyBuyClientId : await GetMeClientId(userWalletId),
                    DestClientId = direct ? await GetMeClientId(userWalletId) : easyBuyClientId,
                    Amount = (double)amount,
                    AssetId = assetId,
                    OperationId = transferId
                });

            if (!result.IsOk())
            {
                if (result.Code == 401)
                {
                    throw new MeNotEnoughFundsException(transferId);
                }
                else
                {
                    throw new MeOperationException(transferId, result.Code);
                }
            }
        }
        
        private async Task<string> GetMeClientId(string walletId)
        {
            if (_walletIdsMapping.TryGetValue(walletId, out var meClientId))
                return meClientId;
            
            var wallet = await _clientAccountClient.GetWalletAsync(walletId);

            meClientId = wallet.Type == WalletType.Trading.ToString() ? wallet.ClientId : walletId;

            _walletIdsMapping[walletId] = meClientId;

            return meClientId;
        }
    }
}

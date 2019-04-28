using System;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Extensions;
using Lykke.Service.ExchangeOperations.Client;
using Lykke.Service.ExchangeOperations.Client.Models;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class ExchangeService : IExchangeService
    {
        private readonly IExchangeOperationsServiceClient _exchangeOperationsServiceClient;
        private readonly ILog _log;

        public ExchangeService(
            IExchangeOperationsServiceClient exchangeOperationsServiceClient,
            ILogFactory logFactory)
        {
            _exchangeOperationsServiceClient = exchangeOperationsServiceClient;
            _log = logFactory.CreateLog(this);
        }

        public async Task<string> TransferAsync(string sourceWalletId, string destinationWalletId, string assetId,
            decimal amount, string transactionId = null)
        {
            _log.InfoWithDetails("Transfer request.",
                new {sourceWalletId, destinationWalletId, assetId, amount, transactionId});

            ExchangeOperationResult result;

            try
            {
                result = await _exchangeOperationsServiceClient.ExchangeOperations.TransferAsync(
                    new TransferRequestModel
                    {
                        SourceClientId = sourceWalletId,
                        DestClientId = destinationWalletId,
                        Amount = (double) amount,
                        AssetId = assetId,
                        OperationId = transactionId
                    });
            }
            catch (Exception exception)
            {
                throw new Exception("An error occurred while processing transfer request.", exception);
            }

            if (result == null)
                throw new Exception("ME result is null.");

            _log.InfoWithDetails("Transfer response.", result);

            if (!result.IsOk())
            {
                if (result.Code == 401)
                    throw new NotEnoughFundsException();

                if (result.Code == 430)
                    throw new DuplicateTransferException();

                throw new ExchangeOperationException("Unexpected transfer response status.");
            }

            return result.TransactionId;
        }
    }
}

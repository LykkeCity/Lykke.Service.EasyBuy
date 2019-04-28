using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client.Models.v3;
using Lykke.Service.Assets.Client.ReadModels;
using Lykke.Service.EasyBuy.Domain.Entities.Balances;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Extensions;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class OrderService : IOrderService
    {
        private readonly IPriceService _priceService;
        private readonly IInstrumentService _instrumentService;
        private readonly IExchangeService _exchangeService;
        private readonly ISettingsService _settingsService;
        private readonly IBalancesService _balancesService;
        private readonly ITransferRepository _transferRepository;
        private readonly IAssetsReadModelRepository _assetsRepository;
        private readonly IAssetPairsReadModelRepository _assetPairsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILog _log;

        public OrderService(
            IPriceService priceService,
            IInstrumentService instrumentService,
            IExchangeService exchangeService,
            IAssetsReadModelRepository assetsRepository,
            IAssetPairsReadModelRepository assetPairsRepository,
            IOrderRepository orderRepository,
            ISettingsService settingsService,
            IBalancesService balancesService,
            ITransferRepository transferRepository,
            ILogFactory logFactory)
        {
            _priceService = priceService;
            _instrumentService = instrumentService;
            _exchangeService = exchangeService;
            _assetsRepository = assetsRepository;
            _assetPairsRepository = assetPairsRepository;
            _orderRepository = orderRepository;
            _settingsService = settingsService;
            _balancesService = balancesService;
            _transferRepository = transferRepository;
            _log = logFactory.CreateLog(this);
        }

        public Task<Order> GetByIdAsync(string orderId)
        {
            return _orderRepository.GetByIdAsync(orderId);
        }

        public Task<IReadOnlyList<Order>> GetAllAsync(string clientId, string assetPair, DateTime? dateFrom,
            DateTime? dateTo, int skip, int take)
        {
            return _orderRepository.GetAsync(clientId, assetPair, dateFrom, dateTo, skip, take);
        }

        public async Task<Order> CreateAsync(string clientId, string priceId, decimal quoteVolume)
        {
            Price price = await _priceService.GetByIdAsync(priceId);

            if (price == null)
                throw new FailedOperationException("Price not found.");

            Instrument instrument = await _instrumentService.GetByAssetPairAsync(price.AssetPair);

            if (instrument.Status != InstrumentStatus.Active)
                throw new FailedOperationException("Instrument inactive.");

            if (price.ValidTo + instrument.OverlapTime < DateTime.UtcNow)
                throw new FailedOperationException("Price expired.");

            if (quoteVolume < instrument.MinQuoteVolume)
                throw new FailedOperationException("Quote volume too small.");

            if (instrument.MaxQuoteVolume < quoteVolume)
                throw new FailedOperationException("Quote volume too much.");

            AssetPair pair = _assetPairsRepository.TryGet(price.AssetPair);

            Asset baseAsset = _assetsRepository.TryGet(pair.BaseAssetId);

            decimal baseVolume = Math.Round(quoteVolume / price.Value, baseAsset.Accuracy);

            var order = new Order(clientId, price.Id, price.AssetPair, baseVolume, quoteVolume);

            await _orderRepository.InsertAsync(order);

            _log.Info("Order created.", order);

            return order;
        }

        public async Task ExecuteAsync()
        {
            IReadOnlyList<Order> orders = await _orderRepository.GetByStatusAsync(OrderStatus.New);

            foreach (Order order in orders.OrderBy(o => o.CreatedDate))
            {
                try
                {
                    bool isValid = await ValidateAsync(order);

                    if (isValid)
                    {
                        await ReserveAsync(order);

                        if (order.Status == OrderStatus.Reserved)
                            await TransferAsync(order);
                    }
                }
                catch (Exception exception)
                {
                    _log.ErrorWithDetails(exception, "An error occurred while processing order.", order);
                }
            }
        }

        private async Task<bool> ValidateAsync(Order order)
        {
            AssetPair assetPair = _assetPairsRepository.TryGet(order.AssetPair);

            Balance balance = await _balancesService.GetByAssetAsync(assetPair.BaseAssetId);

            if (balance.Amount < order.BaseVolume)
            {
                order.Cancel("No liquidity");

                await _orderRepository.UpdateAsync(order);

                _log.WarningWithDetails("Order cancelled due to low balance.", new {Order = order, Balance = balance});

                return false;
            }

            return true;
        }

        private async Task ReserveAsync(Order order)
        {
            AssetPair assetPair = _assetPairsRepository.TryGet(order.AssetPair);

            string error = null;

            Transfer transfer = await EnsureTransferAsync(order.Id, TransferType.Reserve);

            string walletId = _settingsService.GetWalletId();

            try
            {
                await _exchangeService.TransferAsync(order.ClientId, walletId, assetPair.QuotingAssetId,
                    order.QuoteVolume, transfer.Id);
            }
            catch (NotEnoughFundsException)
            {
                error = "No enough funds";
            }
            catch (Exception exception)
            {
                _log.ErrorWithDetails(exception, "An error occurred while reserving client funds.", order);

                error = "Unexpected error";
            }

            if (!string.IsNullOrEmpty(error))
                order.Cancel(error);
            else
                order.Reserved();

            await _orderRepository.UpdateAsync(order);

            _log.InfoWithDetails("Client funds are reserved.", order);
        }

        private async Task TransferAsync(Order order)
        {
            AssetPair assetPair = _assetPairsRepository.TryGet(order.AssetPair);

            Transfer transfer = await EnsureTransferAsync(order.Id, TransferType.Settlement);

            string walletId = _settingsService.GetWalletId();

            bool completed = false;

            try
            {
                await _exchangeService.TransferAsync(walletId, order.ClientId, assetPair.BaseAssetId,
                    order.BaseVolume, transfer.Id);

                completed = true;
            }
            catch (NotEnoughFundsException)
            {
                _log.WarningWithDetails("No enough funds to fill order.", order);
            }
            catch (Exception exception)
            {
                _log.ErrorWithDetails(exception, "An error occurred while transferring funds to client.", order);
            }

            if (!completed)
                return;

            order.Complete();

            await _orderRepository.UpdateAsync(order);

            _log.InfoWithDetails("Order filled.", order);
        }

        private async Task<Transfer> EnsureTransferAsync(string orderId, TransferType transferType)
        {
            Transfer transfer = await _transferRepository.GetAsync(orderId, transferType);

            if (transfer == null)
            {
                transfer = new Transfer(orderId, transferType);
                await _transferRepository.InsertAsync(transfer);
            }

            return transfer;
        }
    }
}

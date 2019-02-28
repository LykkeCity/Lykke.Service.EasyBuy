using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;
using MongoDB.Bson;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class OrdersService : IOrdersService
    {
        private readonly IAssetsServiceWithCache _assetsService;
        private readonly ISettingsService _settingsService;
        private readonly IInternalTransfersService _internalTransfersService;
        private readonly IPricesService _pricesService;
        private readonly IOrdersRepository _ordersRepository;
        private readonly ITradesRepository _tradesRepository;
        private readonly ILog _log;

        public OrdersService(
            IPricesService pricesService,
            IInternalTransfersService internalTransfersService,
            IAssetsServiceWithCache assetsService,
            IOrdersRepository ordersRepository,
            ISettingsService settingsService,
            ITradesRepository tradesRepository,
            ILogFactory logFactory)
        {
            _pricesService = pricesService;
            _internalTransfersService = internalTransfersService;
            _assetsService = assetsService;
            _ordersRepository = ordersRepository;
            _settingsService = settingsService;
            _tradesRepository = tradesRepository;
            _log = logFactory.CreateLog(this);
        }
        
        public async Task<Order> CreateAsync(string walletId, string priceId, decimal quotingVolume)
        {
            var price = await _pricesService.GetSnapshotAsync(priceId);
            
            if(price == null)
                throw new EntityNotFoundException();
            
            if(DateTime.UtcNow > price.ValidTo)
                throw new OperationFailedException("Given price too old.");
            
            if(quotingVolume > price.QuotingVolume)
                throw new OperationFailedException("Requested volume higher than initial.");

            var pair = await _assetsService.TryGetAssetPairAsync(price.AssetPair);

            var baseAsset = await _assetsService.TryGetAssetAsync(pair.BaseAssetId);

            var baseVolume = quotingVolume == price.QuotingVolume
                ? price.BaseVolume
                : (quotingVolume / price.Price).TruncateDecimalPlaces(baseAsset.Accuracy, price.Type == OrderType.Sell);
            
            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                WalletId = walletId,
                Type = price.Type,
                AssetPair = price.AssetPair,
                QuotingVolume = quotingVolume,
                BaseVolume = baseVolume,
                PriceId = priceId,
                CreatedTime = DateTime.UtcNow,
                Status = OrderStatus.New,
                ReserveTransferId = Guid.NewGuid().ToString(),
                SettlementTransferId = Guid.NewGuid().ToString()
            };

            await _ordersRepository.InsertAsync(order);
            
            try
            {
                await _internalTransfersService.TransferAsync(
                    order.ReserveTransferId,
                    await _settingsService.GetClientIdAsync(),
                    walletId,
                    true,
                    order.Type == OrderType.Buy
                        ? pair.QuotingAssetId
                        : pair.BaseAssetId,
                    order.Type == OrderType.Buy
                        ? quotingVolume
                        : baseVolume);

            }
            catch (MeNotEnoughFundsException)
            {
                await PersistWithStatusAsync(order, OrderStatus.Cancelled);

                throw new OperationFailedException("Client doesn't have enough funds.");
            }
            catch (MeOperationException e)
            {
                await PersistWithStatusAsync(order, OrderStatus.Cancelled);

                _log.Error(e);
                
                throw new OperationFailedException("ME call failed.");
            }

            await PersistWithStatusAsync(order, OrderStatus.Reserved);

            return order;
        }
        
        private async Task PersistWithStatusAsync(Order order, OrderStatus status)
        {
            order.Status = status;
            
            await PersistOrderAsync(order);
        }
        
        private Task PersistOrderAsync(Order order)
        {
            return _ordersRepository.UpdateAsync(order);
        }

        public async Task ProcessPendingAsync()
        {
            var executedOrders = await _ordersRepository.GetByStatusAsync(OrderStatus.Reserved);

            foreach (var order in executedOrders)
            {
                var easyBuyClientId = await _settingsService.GetClientIdAsync();
                
                var pair = await _assetsService.TryGetAssetPairAsync(order.AssetPair);

                try
                {
                    await _internalTransfersService.TransferAsync(
                        order.SettlementTransferId,
                        easyBuyClientId,
                        order.WalletId,
                        false,
                        order.Type == OrderType.Buy
                            ? pair.BaseAssetId
                            : pair.QuotingAssetId,
                        order.Type == OrderType.Buy
                            ? order.BaseVolume
                            : order.QuotingVolume);

                    await PersistWithStatusAsync(order, OrderStatus.Completed);

                    await _tradesRepository.InsertAsync(new Trade
                    {
                        Id = Guid.NewGuid().ToString(),
                        BaseVolume = order.BaseVolume,
                        QuotingVolume = order.QuotingVolume,
                        WalletId = order.WalletId,
                        OrderId = order.Id,
                        Type = order.Type,
                        DateTime = DateTime.UtcNow
                    });
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }

        public async Task<Order> GetAsync(string walletId, string id)
        {
            var order = await _ordersRepository.GetAsync(walletId, id);

            if (order == null)
                throw new EntityNotFoundException(nameof(id), id);

            return order;
        }
    }
}

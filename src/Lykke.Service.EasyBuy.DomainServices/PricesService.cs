using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Service.Assets.Client;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class PricesService : IPricesService
    {
        private readonly IAssetsServiceWithCache _assetsService;
        private readonly IInstrumentsService _instrumentsService;
        private readonly IOrderBookService _orderBookService;
        private readonly IPriceSnapshotsRepository _priceSnapshotsRepository;
        private readonly ISettingsService _settingsService;
        
        public PricesService(
            IAssetsServiceWithCache assetsService,
            IInstrumentsService instrumentsService,
            IOrderBookService orderBookService,
            IPriceSnapshotsRepository priceSnapshotsRepository,
            ISettingsService settingsService)
        {
            _assetsService = assetsService;
            _instrumentsService = instrumentsService;
            _orderBookService = orderBookService;
            _priceSnapshotsRepository = priceSnapshotsRepository;
            _settingsService = settingsService;
        }
        
        public async Task<PriceSnapshot> CreateSnapshotAsync(string walletId, string assetPair, OrderType type, decimal quotingVolume)
        {
            var instrument = await _instrumentsService.GetByAssetPairIdAsync(assetPair);

            if (instrument == null || instrument.State != InstrumentState.Active)
            {
                throw new OperationFailedException($"No active instrument {assetPair} was found.");
            }

            var orderBook = _orderBookService.GetByAssetPairId(instrument.Exchange, assetPair);

            if(type == OrderType.None)
                throw new OperationFailedException($"Invalid order type {OrderType.None}");

            var pair = await _assetsService.TryGetAssetPairAsync(assetPair);
            
            if(pair == null)
                throw new OperationFailedException($"Pair {assetPair} not found.");

            var baseAsset = await _assetsService.TryGetAssetAsync(pair.BaseAssetId);
            var quotingAsset = await _assetsService.TryGetAssetAsync(pair.QuotingAssetId);
            
            if(baseAsset == null)
                throw new OperationFailedException($"Base asset {pair.BaseAssetId} not found.");
            
            if(quotingAsset == null)
                throw new OperationFailedException($"Quoting asset {pair.QuotingAssetId} not found.");

            var orders = type == OrderType.Buy
                ? orderBook.SellLimitOrders.OrderBy(x => x.Price).ToList()
                : orderBook.BuyLimitOrders.OrderByDescending(x => x.Price).ToList();
            
            if (!orders.Any() || orders.Sum(x => x.Volume * x.Price) < quotingVolume)
            {
                throw new OperationFailedException("Not enough liquidity.");
            }

            var markup = instrument.Markup ?? await _settingsService.GetDefaultMarkupAsync();
            
            var ordersInUsd = orders.Select(x => x.Volume * (x.Price + markup * (type == OrderType.Sell ? -1 : 1))).ToList();
                
            var volumes = new List<decimal>();
            var remainingVolume = quotingVolume;

            for (var i = 0; i < ordersInUsd.Count; i++)
            {
                var orderVolumeInUsd = ordersInUsd[i];
                    
                if (remainingVolume == 0m || remainingVolume <= (decimal) pair.MinVolume)
                    break;

                if (orderVolumeInUsd <= remainingVolume)
                {
                    volumes.Add(orders[i].Volume);

                    remainingVolume -= orderVolumeInUsd;
                }
                else
                {
                    volumes.Add(remainingVolume / (orders[i].Price + markup * (type == OrderType.Sell ? -1 : 1)));

                    break;
                }
            }

            var volumeBase = volumes.Sum().TruncateDecimalPlaces(baseAsset.Accuracy, type == OrderType.Sell);
            
            var price = (quotingVolume / volumeBase).TruncateDecimalPlaces(pair.Accuracy, type == OrderType.Buy);

            var priceLifetime = instrument.PriceLifetime ?? await _settingsService.GetDefaultPriceLifetimeAsync();

            var validFrom = DateTime.UtcNow;
            var validTo = validFrom + priceLifetime;

            var priceSnapshot = new PriceSnapshot
            {
                Id = Guid.NewGuid().ToString(),
                WalletId = walletId,
                Price = price,
                AssetPair = assetPair,
                Exchange = instrument.Exchange,
                BaseVolume = volumeBase,
                QuotingVolume = quotingVolume,
                Markup = markup,
                OriginalPrice = price - markup,
                Type = type,
                ValidFrom = validFrom,
                ValidTo = validTo
            };

            await _priceSnapshotsRepository.InsertAsync(priceSnapshot);
            
            return priceSnapshot;
        }

        public async Task<PriceSnapshot> GetSnapshotAsync(string id)
        {
            var priceSnapshot = await _priceSnapshotsRepository.GetAsync(id);
            
            if (priceSnapshot == null)
                throw new EntityNotFoundException();

            return priceSnapshot;
        }
        
        private static decimal Floor(decimal value, int precision)
        {
            var multiplier = (decimal) Math.Pow(10, precision);
            
            return Math.Floor(value * multiplier) / multiplier;
        }
    }
}

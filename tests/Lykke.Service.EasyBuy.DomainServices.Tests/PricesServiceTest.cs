using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.EasyBuy.DomainServices.Tests
{
    [TestClass]
    public class PricesServiceTest
    {
        private readonly Mock<IAssetsServiceWithCache> _assetsServiceMock = new Mock<IAssetsServiceWithCache>();
        
        private readonly Mock<IInstrumentsService> _instrumentsServiceMock = new Mock<IInstrumentsService>();
        
        private readonly Mock<IOrderBookService> _orderBookServiceMock = new Mock<IOrderBookService>();
        
        private readonly Mock<IPriceSnapshotsRepository> _pricesRepositoryMock = new Mock<IPriceSnapshotsRepository>();
        
        private readonly Mock<ISettingsService> _settingsServiceMock = new Mock<ISettingsService>();

        private IPricesService _pricesService;
        
        [TestMethod]
        public void Test_User_Buy_One_Level()
        {
            // arrange

            var walletId = Guid.NewGuid().ToString();
            var assetPair = "BTCUSD";
            var orderType = OrderType.Buy;
            var baseVolume = 35m;
            
            // act

            var priceSnapshot = _pricesService
                .CreateSnapshotAsync(walletId, assetPair, orderType, baseVolume)
                .GetAwaiter()
                .GetResult();

            // assert
            
            Assert.AreEqual(0.00579470m, priceSnapshot.BaseVolume);
            Assert.AreEqual(6040.01m, priceSnapshot.Price);
        }
        
        [TestMethod]
        public void Test_User_Buy_Multiple_Levels()
        {
            // arrange

            var walletId = Guid.NewGuid().ToString();
            var assetPair = "BTCUSD";
            var orderType = OrderType.Buy;
            var baseVolume = 300m;
            
            // act

            var priceSnapshot = _pricesService
                .CreateSnapshotAsync(walletId, assetPair, orderType, baseVolume)
                .GetAwaiter()
                .GetResult();

            // assert
            
            Assert.AreEqual(0.04961056m, priceSnapshot.BaseVolume);
            Assert.AreEqual(6047.1m, priceSnapshot.Price);
        }

        [TestMethod]
        public void Test_User_Sell_One_Level()
        {
            // arrange

            var walletId = Guid.NewGuid().ToString();
            var assetPair = "BTCUSD";
            var orderType = OrderType.Sell;
            var baseVolume = 35m;
            
            // act

            var priceSnapshot = _pricesService
                .CreateSnapshotAsync(walletId, assetPair, orderType, baseVolume)
                .GetAwaiter()
                .GetResult();

            // assert
            
            Assert.AreEqual(0.00584113m, priceSnapshot.BaseVolume);
            Assert.AreEqual(5991.99m, priceSnapshot.Price);
        }

        [TestMethod]
        public void Test_User_Sell_Multiple_Levels()
        {
            // arrange

            var walletId = Guid.NewGuid().ToString();
            var assetPair = "BTCUSD";
            var orderType = OrderType.Sell;
            var baseVolume = 300m;
            
            // act

            var priceSnapshot = _pricesService
                .CreateSnapshotAsync(walletId, assetPair, orderType, baseVolume)
                .GetAwaiter()
                .GetResult();

            // assert
            
            Assert.AreEqual(0.05007279m, priceSnapshot.BaseVolume);
            Assert.AreEqual(5991.27m, priceSnapshot.Price);
        }
        
        
        [TestInitialize]
        public void TestInitialize()
        {
            SetupAssetsService();

            SetupInstrumentsService();

            SetupOrderBookService();

            SetupPricesRepository();

            SetupSettingsService();

            SetupPricesService();
        }

        private void SetupPricesService()
        {
            _pricesService = new PricesService(
                _assetsServiceMock.Object,
                _instrumentsServiceMock.Object,
                _orderBookServiceMock.Object,
                _pricesRepositoryMock.Object,
                _settingsServiceMock.Object);
        }

        private void SetupSettingsService()
        {
            _settingsServiceMock.Setup(x => x.GetDefaultMarkupAsync())
                .Returns(() => Task.FromResult(10m));
            
            _settingsServiceMock.Setup(x => x.GetDefaultPriceLifetimeAsync())
                .Returns(() => Task.FromResult(TimeSpan.FromSeconds(20)));
        }

        private void SetupPricesRepository()
        {
            _pricesRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<PriceSnapshot>()))
                .Returns(() => Task.CompletedTask);
        }

        private void SetupOrderBookService()
        {
            _orderBookServiceMock.Setup(x => x.GetByAssetPairId("NettingEngineDefault", "BTCUSD"))
                .Returns(() =>
                    new OrderBook
                    {
                        AssetPair = "BTCUSD",
                        Timestamp = DateTime.UtcNow,
                        SellLimitOrders = new List<OrderBookLimitOrder>
                        {
                            new OrderBookLimitOrder
                            {
                                Volume = 0.032m,
                                Price = 6030m
                            },
                            new OrderBookLimitOrder
                            {
                                Volume = 0.053m,
                                Price = 6050m
                            },
                            new OrderBookLimitOrder
                            {
                                Volume = 0.1m,
                                Price = 6100m
                            }
                        },
                        BuyLimitOrders = new List<OrderBookLimitOrder>
                        {
                            new OrderBookLimitOrder
                            {
                                Volume = 0.032m,
                                Price = 6002m
                            },
                            new OrderBookLimitOrder
                            {
                                Volume = 0.1m,
                                Price = 6000m
                            },
                            new OrderBookLimitOrder
                            {
                                Volume = 0.1m,
                                Price = 5800m
                            },
                            new OrderBookLimitOrder
                            {
                                Volume = 0.2m,
                                Price = 5700m
                            },
                            new OrderBookLimitOrder
                            {
                                Volume = 0.1m,
                                Price = 5500m
                            }
                        }
                    });
        }

        private void SetupInstrumentsService()
        {
            _instrumentsServiceMock.Setup(x => x.GetByAssetPairIdAsync("BTCUSD"))
                .Returns(() => Task.FromResult(new Instrument
                {
                    AssetPair = "BTCUSD",
                    Exchange = "NettingEngineDefault",
                    Markup = null,
                    PriceLifetime = TimeSpan.FromSeconds(22),
                    State = InstrumentState.Active
                }));
        }

        private void SetupAssetsService()
        {
            _assetsServiceMock.Setup(x => x.TryGetAssetPairAsync("BTCUSD", default(CancellationToken)))
                .Returns(() => Task.FromResult(new AssetPair
                {
                    Id = "BTCUSD",
                    BaseAssetId = "BTC",
                    QuotingAssetId = "USD",
                    MinVolume = 0.00001,
                    Accuracy = 2
                }));

            _assetsServiceMock.Setup(x => x.TryGetAssetAsync("BTC", default(CancellationToken)))
                .Returns(() => Task.FromResult(new Asset
                {
                    Id = "BTC",
                    Accuracy = 8
                }));

            _assetsServiceMock.Setup(x => x.TryGetAssetAsync("USD", default(CancellationToken)))
                .Returns(() => Task.FromResult(new Asset
                {
                    Id = "USD",
                    Accuracy = 2
                }));
        }
    }
}

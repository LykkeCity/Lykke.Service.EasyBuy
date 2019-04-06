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
        
        private readonly Mock<IInstrumentsAccessService> _instrumentsAccessServiceMock = new Mock<IInstrumentsAccessService>();
        
        private readonly Mock<IOrderBookService> _orderBookServiceMock = new Mock<IOrderBookService>();
        
        private readonly Mock<IPricesRepository> _pricesRepositoryMock = new Mock<IPricesRepository>();
        
        private readonly Mock<ISettingsService> _settingsServiceMock = new Mock<ISettingsService>();

        private IPricesService _pricesService;
        
        [TestMethod]
        public void Test_User_Buy_One_Level()
        {
            // arrange
            
            var assetPair = "BTCUSD";
            var orderType = OrderType.Buy;
            var baseVolume = 35m;
            
            // act

            var priceSnapshot = _pricesService
                .CreateAsync(assetPair, orderType, baseVolume, DateTime.UtcNow)
                .GetAwaiter()
                .GetResult();

            // assert
            
            Assert.AreEqual(0.00580431m, priceSnapshot.BaseVolume);
            Assert.AreEqual(6150.32m, priceSnapshot.Value);
        }
        
        [TestMethod]
        public void Test_User_Buy_Multiple_Levels()
        {
            // arrange
            
            var assetPair = "BTCUSD";
            var orderType = OrderType.Buy;
            var baseVolume = 300m;
            
            // act

            var priceSnapshot = _pricesService
                .CreateAsync(assetPair, orderType, baseVolume, DateTime.UtcNow)
                .GetAwaiter()
                .GetResult();

            // assert
            
            Assert.AreEqual(0.04969256m, priceSnapshot.BaseVolume);
            Assert.AreEqual(6157.45m, priceSnapshot.Value);
        }

        [TestMethod]
        public void Test_User_Sell_One_Level()
        {
            // arrange
            
            var assetPair = "BTCUSD";
            var orderType = OrderType.Sell;
            var baseVolume = 35m;
            
            // act

            var priceSnapshot = _pricesService
                .CreateAsync(assetPair, orderType, baseVolume, DateTime.UtcNow)
                .GetAwaiter()
                .GetResult();

            // assert
            
            Assert.AreEqual(0.00583139m, priceSnapshot.BaseVolume);
            Assert.AreEqual(5881.67m, priceSnapshot.Value);
        }

        [TestMethod]
        public void Test_User_Sell_Multiple_Levels()
        {
            // arrange
            
            var assetPair = "BTCUSD";
            var orderType = OrderType.Sell;
            var baseVolume = 300m;
            
            // act

            var priceSnapshot = _pricesService
                .CreateAsync(assetPair, orderType, baseVolume, DateTime.UtcNow)
                .GetAwaiter()
                .GetResult();

            // assert
            
            Assert.AreEqual(0.04998933m, priceSnapshot.BaseVolume);
            Assert.AreEqual(5880.96m, priceSnapshot.Value);
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
                _instrumentsAccessServiceMock.Object,
                _orderBookServiceMock.Object,
                _pricesRepositoryMock.Object,
                _settingsServiceMock.Object);
        }

        private void SetupSettingsService()
        {
            _settingsServiceMock.Setup(x => x.GetDefaultSettingsAsync())
                .Returns(() => Task.FromResult(new DefaultSetting
                {
                    Markup = 0.02m,
                    OverlapTime = TimeSpan.Zero,
                    PriceLifetime = TimeSpan.FromSeconds(20),
                    RecalculationInterval = TimeSpan.Zero,
                    OrdersProcessorPeriod = TimeSpan.FromSeconds(5)
                }));
        }

        private void SetupPricesRepository()
        {
            _pricesRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<Price>()))
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
            _instrumentsAccessServiceMock.Setup(x => x.GetByAssetPairIdAsync("BTCUSD"))
                .Returns(() => Task.FromResult(new Instrument
                {
                    AssetPair = "BTCUSD",
                    Exchange = "NettingEngineDefault",
                    Markup = null,
                    PriceLifetime = TimeSpan.FromSeconds(20),
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

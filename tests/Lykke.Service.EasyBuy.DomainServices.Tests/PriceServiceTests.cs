using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Logs;
using Lykke.Service.Assets.Client.Models.v3;
using Lykke.Service.Assets.Client.ReadModels;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Entities.OrderBooks;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.EasyBuy.DomainServices.Tests
{
    [TestClass]
    public class PriceServiceTests
    {
        private readonly Mock<IPriceRepository> _priceRepositoryMock =
            new Mock<IPriceRepository>();

        private readonly Mock<IOrderBookService> _orderBookServiceMock =
            new Mock<IOrderBookService>();

        private readonly Mock<IInstrumentService> _instrumentServiceMock =
            new Mock<IInstrumentService>();

        private readonly Mock<ISettingsService> _settingsServiceMock =
            new Mock<ISettingsService>();

        private readonly Mock<IAssetsReadModelRepository> _assetsRepositoryMock =
            new Mock<IAssetsReadModelRepository>();

        private readonly Mock<IAssetPairsReadModelRepository> _assetPairsRepositoryMock =
            new Mock<IAssetPairsReadModelRepository>();

        private readonly Mock<IPricesPublisher> _pricesPublisherMock =
            new Mock<IPricesPublisher>();

        private OrderBook _orderBook;

        private Instrument _instrument;

        private Price _currentPrice;

        private TimeSpan _recalculationInterval;

        private IPriceService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            const string exchangeName = "Lykke";
            const string assetName = "BTC";
            const string assetPairName = "BTCUSD";

            _orderBook = new OrderBook
            {
                Exchange = exchangeName,
                AssetPair = assetPairName,
                Timestamp = DateTime.UtcNow,
                SellLevels = new List<OrderBookLevel>
                {
                    new OrderBookLevel {Price = 5187.4m, Volume = 2.09m},
                    new OrderBookLevel {Price = 5187.1m, Volume = 1.66m},
                    new OrderBookLevel {Price = 5186.9m, Volume = 1.46m},
                    new OrderBookLevel {Price = 5186.5m, Volume = 1.4m},
                    new OrderBookLevel {Price = 5185.8m, Volume = 0.4m}
                },
                BuyLevels = new List<OrderBookLevel>()
            };

            _instrument = new Instrument
            {
                Id = Guid.NewGuid().ToString(),
                AssetPair = assetPairName,
                Exchange = exchangeName,
                Lifetime = TimeSpan.FromSeconds(20),
                OverlapTime = TimeSpan.FromSeconds(1),
                Markup = 0.05m,
                MinQuoteVolume = 50,
                MaxQuoteVolume = 10000,
                Status = InstrumentStatus.Active
            };

            _recalculationInterval = TimeSpan.FromMilliseconds(500);

            _orderBookServiceMock.Setup(o => o.GetByAssetPair(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string exchange, string assetPair) => _orderBook.AssetPair == assetPair ? _orderBook : null);

            _instrumentServiceMock.Setup(o => o.GetByAssetPairAsync(It.IsAny<string>()))
                .Returns((string assetPair) =>
                    Task.FromResult(_instrument.AssetPair == assetPair
                        ? _instrument
                        : null));

            _priceRepositoryMock.Setup(o => o.GetLatestAsync())
                .Returns(() =>
                    Task.FromResult<IReadOnlyList<Price>>(_currentPrice != null
                        ? new[] {_currentPrice}
                        : new Price[0]));

            _settingsServiceMock.Setup(o => o.GetRecalculationInterval())
                .Returns(() => _recalculationInterval);

            _assetsRepositoryMock.Setup(o => o.TryGet(It.Is<string>(assetId => assetName == assetId)))
                .Returns((string assetId) => new Asset
                {
                    Id = assetName,
                    Accuracy = 8
                });

            _assetPairsRepositoryMock.Setup(o => o.TryGet(It.Is<string>(assetPair => assetPairName == assetPair)))
                .Returns((string assetId) => new AssetPair
                {
                    Id = assetPairName,
                    Name = assetPairName,
                    BaseAssetId = assetName,
                    Accuracy = 3,
                    InvertedAccuracy = 8,
                    MinVolume = 0.00001m
                });

            _service = new PriceService(
                _priceRepositoryMock.Object,
                _orderBookServiceMock.Object,
                _instrumentServiceMock.Object,
                _settingsServiceMock.Object,
                _assetsRepositoryMock.Object,
                _assetPairsRepositoryMock.Object,
                _pricesPublisherMock.Object,
                EmptyLogFactory.Instance);
        }

        [TestMethod]
        public async Task Save_Price()
        {
            // arrange

            // act

            await _service.EnsureAsync(_orderBook.AssetPair, DateTime.UtcNow);

            // assert

            _priceRepositoryMock.Verify(o => o.InsertAsync(It.IsAny<Price>()), Times.Once);
        }

        [TestMethod]
        public async Task Publish_Price()
        {
            // arrange

            // act

            await _service.EnsureAsync(_orderBook.AssetPair, DateTime.UtcNow);

            // assert

            _pricesPublisherMock.Verify(o => o.PublishAsync(It.IsAny<Price>()), Times.Once);
        }

        [TestMethod]
        public async Task Calculate_New_Price()
        {
            // arrange

            _currentPrice = null;

            // act

            await _service.EnsureAsync(_orderBook.AssetPair, DateTime.UtcNow);

            Price price = await _service.GetByAssetPair(_orderBook.AssetPair);

            // assert

            Assert.IsNotNull(price);
        }

        [TestMethod]
        public async Task Calculate_Next_Price_If_Prev_Price_Valid_To_Is_Less_Than_Current_Date()
        {
            // arrange

            DateTime currentDate = DateTime.UtcNow;

            _currentPrice = new Price
            {
                Id = Guid.NewGuid().ToString(),
                AssetPair = _orderBook.AssetPair,
                ValidTo = currentDate.AddMilliseconds(-_recalculationInterval.TotalMilliseconds / 2d)
            };

            // act

            await _service.EnsureAsync(_orderBook.AssetPair, currentDate);

            Price price = await _service.GetByAssetPair(_orderBook.AssetPair);

            // assert

            Assert.AreEqual(price.ValidFrom, _currentPrice.ValidTo);
        }

        [TestMethod]
        public async Task Calculate_Next_Price_If_Prev_Price_Valid_To_Is_Greater_Than_Current_Date()
        {
            // arrange

            DateTime currentDate = DateTime.UtcNow;

            _currentPrice = new Price
            {
                Id = Guid.NewGuid().ToString(),
                AssetPair = _orderBook.AssetPair,
                ValidTo = currentDate.AddMilliseconds(_recalculationInterval.TotalMilliseconds / 2d)
            };

            // act

            await _service.EnsureAsync(_orderBook.AssetPair, currentDate);

            Price price = await _service.GetByAssetPair(_orderBook.AssetPair);

            // assert

            Assert.AreEqual(price.ValidFrom, _currentPrice.ValidTo);
        }

        [TestMethod]
        public async Task Calculate_New_Price_If_Prev_Price_Valid_To_Is_Less_Than_Current_Date()
        {
            // arrange

            DateTime currentDate = DateTime.UtcNow;

            _currentPrice = new Price
            {
                Id = Guid.NewGuid().ToString(),
                AssetPair = _orderBook.AssetPair,
                ValidTo = currentDate.AddMilliseconds(-(_recalculationInterval.TotalMilliseconds + 100))
            };

            // act

            await _service.EnsureAsync(_orderBook.AssetPair, currentDate);

            Price price = await _service.GetByAssetPair(_orderBook.AssetPair);

            // assert

            Assert.AreEqual(price.ValidFrom, currentDate);
        }

        [TestMethod]
        public async Task Do_Not_Calculate_Next_Price_If_Prev_Price_Valid_To_Is_Greater_Than_Current_Date()
        {
            // arrange

            DateTime currentDate = DateTime.UtcNow;

            _currentPrice = new Price
            {
                Id = Guid.NewGuid().ToString(),
                AssetPair = _orderBook.AssetPair,
                ValidTo = currentDate.AddMilliseconds(_recalculationInterval.TotalMilliseconds + 100)
            };

            // act

            await _service.EnsureAsync(_orderBook.AssetPair, currentDate);

            Price price = await _service.GetByAssetPair(_orderBook.AssetPair);

            // assert

            Assert.AreEqual(price.Id, _currentPrice.Id);
        }

        [TestMethod]
        public async Task Do_Not_Calculate_Price_If_Instrument_Disabled()
        {
            // arrange

            DateTime currentDate = DateTime.UtcNow;

            _currentPrice = new Price
            {
                Id = Guid.NewGuid().ToString(),
                AssetPair = _orderBook.AssetPair,
                ValidTo = currentDate.AddMilliseconds(-(_recalculationInterval.TotalMilliseconds + 100))
            };

            _instrument.Status = InstrumentStatus.Disabled;

            // act

            await _service.EnsureAsync(_orderBook.AssetPair, currentDate);

            Price price = await _service.GetByAssetPair(_orderBook.AssetPair);

            // assert

            Assert.AreEqual(price.Id, _currentPrice.Id);
        }
    }
}

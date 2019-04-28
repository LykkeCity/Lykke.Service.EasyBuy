using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Logs;
using Lykke.Service.Assets.Client.Models.v3;
using Lykke.Service.Assets.Client.ReadModels;
using Lykke.Service.EasyBuy.Domain.Entities.Balances;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.EasyBuy.DomainServices.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private const string AssetPair = "BTCUSD";
        private const string BaseAsset = "BTC";
        private const string QuoteAsset = "USD";
        private const string WalletId = "wallet_id";

        private readonly Mock<IPriceService> _priceServiceMock =
            new Mock<IPriceService>();

        private readonly Mock<IInstrumentService> _instrumentServiceMock =
            new Mock<IInstrumentService>();

        private readonly Mock<IExchangeService> _exchangeServiceMock =
            new Mock<IExchangeService>();

        private readonly Mock<ISettingsService> _settingsServiceMock =
            new Mock<ISettingsService>();

        private readonly Mock<IBalancesService> _balancesServiceMock =
            new Mock<IBalancesService>();

        private readonly Mock<ITransferRepository> _transferRepositoryMock =
            new Mock<ITransferRepository>();

        private readonly Mock<IAssetsReadModelRepository> _assetsRepositoryMock =
            new Mock<IAssetsReadModelRepository>();

        private readonly Mock<IAssetPairsReadModelRepository> _assetPairsRepositoryMock =
            new Mock<IAssetPairsReadModelRepository>();

        private readonly Mock<IOrderRepository> _orderRepositoryMock =
            new Mock<IOrderRepository>();

        private readonly List<Instrument> _instruments = new List<Instrument>
        {
            new Instrument
            {
                Id = Guid.NewGuid().ToString(),
                AssetPair = AssetPair,
                Lifetime = TimeSpan.FromSeconds(10),
                OverlapTime = TimeSpan.FromSeconds(5),
                Markup = .1m,
                MinQuoteVolume = 50m,
                MaxQuoteVolume = 1000m,
                Status = InstrumentStatus.Active
            }
        };

        private readonly List<Asset> _assets = new List<Asset>
        {
            new Asset
            {
                Id = BaseAsset,
                Accuracy = 8
            },
            new Asset
            {
                Id = QuoteAsset,
                Accuracy = 2
            }
        };

        private readonly List<AssetPair> _assetPairs = new List<AssetPair>
        {
            new AssetPair
            {
                Id = AssetPair,
                Accuracy = 3,
                InvertedAccuracy = 8,
                Name = AssetPair,
                BaseAssetId = BaseAsset,
                QuotingAssetId = QuoteAsset
            }
        };

        private readonly Price _price = new Price
        {
            Id = Guid.NewGuid().ToString(),
            AssetPair = AssetPair,
            Value = 6124.352m,
            BaseVolume = 0.16328258m,
            QuoteVolume = 1000,
            ValidFrom = DateTime.UtcNow.AddSeconds(-1),
            ValidTo = DateTime.UtcNow.AddSeconds(10)
        };

        private readonly List<Order> _orders = new List<Order>();

        private readonly List<Balance> _balances = new List<Balance>();

        private IOrderService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _priceServiceMock.Setup(o => o.GetByIdAsync(It.IsAny<string>()))
                .Returns((string priceId) => Task.FromResult(_price?.Id == priceId ? _price : null));

            _instrumentServiceMock.Setup(o => o.GetByAssetPairAsync(It.IsAny<string>()))
                .Returns((string assetPair) =>
                    Task.FromResult(_instruments.FirstOrDefault(o => o.AssetPair == assetPair)));

            _assetsRepositoryMock.Setup(o => o.TryGet(It.IsAny<string>()))
                .Returns((string asset) => _assets.FirstOrDefault(o => o.Id == asset));

            _assetPairsRepositoryMock.Setup(o => o.TryGet(It.IsAny<string>()))
                .Returns((string assetPair) => _assetPairs.FirstOrDefault(o => o.Id == assetPair));

            _orderRepositoryMock.Setup(o => o.GetByStatusAsync(It.IsAny<OrderStatus>()))
                .Returns((OrderStatus orderStatus) =>
                    Task.FromResult<IReadOnlyList<Order>>(_orders.Where(o => o.Status == orderStatus).ToList()));

            _balancesServiceMock.Setup(o => o.GetByAssetAsync(It.IsAny<string>()))
                .Returns((string asset) =>
                    Task.FromResult(_balances.FirstOrDefault(o => o.Asset == asset) ?? new Balance(asset)));

            _settingsServiceMock.Setup(o => o.GetWalletId())
                .Returns(WalletId);

            _service = new OrderService(
                _priceServiceMock.Object,
                _instrumentServiceMock.Object,
                _exchangeServiceMock.Object,
                _assetsRepositoryMock.Object,
                _assetPairsRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _settingsServiceMock.Object,
                _balancesServiceMock.Object,
                _transferRepositoryMock.Object,
                EmptyLogFactory.Instance);
        }

        [TestMethod]
        public async Task Create_Order()
        {
            // arrange

            string clientId = "me";
            string priceId = _price.Id;
            decimal quoteVolume = 500;
            decimal baseVolume = 0.08164129m;

            var expectedOrder = new Order(clientId, priceId, AssetPair, baseVolume, quoteVolume);

            // act

            Order actualOrder = await _service.CreateAsync(clientId, priceId, quoteVolume);

            // assert

            Assert.IsTrue(AreEqual(expectedOrder, actualOrder));
        }

        [TestMethod]
        public async Task Create_Order_Using_Expired_Price_In_Overlap_Interval()
        {
            // arrange

            string clientId = "me";
            string priceId = _price.Id;
            decimal quoteVolume = 1000;
            decimal baseVolume = 0.16328258m;

            _price.ValidTo = DateTime.UtcNow.AddSeconds(-1);

            var expectedOrder = new Order(clientId, priceId, AssetPair, baseVolume, quoteVolume);

            // act

            Order actualOrder = await _service.CreateAsync(clientId, priceId, quoteVolume);

            // assert

            Assert.IsTrue(AreEqual(expectedOrder, actualOrder));
        }

        [TestMethod]
        [ExpectedException(typeof(FailedOperationException), "Price not found.")]
        public async Task Do_Not_Create_Order_If_Invalid_Price()
        {
            // arrange

            string clientId = "me";
            string priceId = Guid.NewGuid().ToString();
            decimal quoteVolume = 500;

            // act

            await _service.CreateAsync(clientId, priceId, quoteVolume);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedOperationException), "Instrument inactive.")]
        public async Task Do_Not_Create_Order_If_Instrument_Is_Disabled()
        {
            // arrange

            string clientId = "me";
            string priceId = _price.Id;
            decimal quoteVolume = 500;

            Instrument instrument = _instruments.First(o => o.AssetPair == AssetPair);
            instrument.Status = InstrumentStatus.Disabled;

            // act

            await _service.CreateAsync(clientId, priceId, quoteVolume);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedOperationException), "Quote volume too small.")]
        public async Task Do_Not_Create_Order_If_Quote_Volume_Is_Too_Small()
        {
            // arrange

            string clientId = "me";
            string priceId = _price.Id;
            decimal quoteVolume = 5;

            // act

            await _service.CreateAsync(clientId, priceId, quoteVolume);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedOperationException), "Quote volume too much.")]
        public async Task Do_Not_Create_Order_If_Quote_Volume_Is_Too_Much()
        {
            // arrange

            string clientId = "me";
            string priceId = _price.Id;
            decimal quoteVolume = 5000;

            // act

            await _service.CreateAsync(clientId, priceId, quoteVolume);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedOperationException), "Price expired.")]
        public async Task Do_Not_Create_Order_If_Price_Expired()
        {
            // arrange

            string clientId = "me";
            string priceId = _price.Id;
            decimal quoteVolume = 1000;

            _price.ValidTo = DateTime.UtcNow.AddSeconds(-30);

            // act

            await _service.CreateAsync(clientId, priceId, quoteVolume);
        }

        [TestMethod]
        public async Task Execute_Order()
        {
            // arrange

            var order = new Order("me", _price.Id, AssetPair, 1, 100);

            _orders.Add(order);

            _balances.Add(new Balance(BaseAsset, 1, 0));

            // act

            await _service.ExecuteAsync();

            // assert

            Assert.AreEqual(order.Status, OrderStatus.Completed);
        }

        [TestMethod]
        public async Task Cancel_Order_If_No_Enough_Liquidity()
        {
            // arrange

            var order = new Order("me", _price.Id, AssetPair, 1, 100);

            _orders.Add(order);

            _balances.Add(new Balance(BaseAsset, .5m, 0));

            // act

            await _service.ExecuteAsync();

            // assert

            Assert.IsTrue(order.Status == OrderStatus.Cancelled && order.Error == "No liquidity");
        }

        [TestMethod]
        public async Task Cancel_Order_If_Client_Has_No_Enough_Funds()
        {
            // arrange

            var order = new Order("me", _price.Id, AssetPair, 1, 100);

            _orders.Add(order);

            _balances.Add(new Balance(BaseAsset, 1m, 0));

            _exchangeServiceMock.Setup(o => o.TransferAsync(It.Is<string>(clientId => clientId == order.ClientId),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>()))
                .Throws(new NotEnoughFundsException());

            // act

            await _service.ExecuteAsync();

            // assert

            Assert.IsTrue(order.Status == OrderStatus.Cancelled && order.Error == "No enough funds");
        }

        private static bool AreEqual(Order a, Order b)
        {
            if (a == null && b == null)
                return true;

            if (a != null && b == null)
                return false;

            if (a == null)
                return false;

            return a.ClientId == b.ClientId &&
                   a.AssetPair == b.AssetPair &&
                   a.BaseVolume == b.BaseVolume &&
                   a.PriceId == b.PriceId &&
                   a.QuoteVolume == b.QuoteVolume;
        }
    }
}

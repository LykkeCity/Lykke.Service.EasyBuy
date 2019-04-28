using System;
using System.Collections.Generic;
using Lykke.Service.EasyBuy.Domain.Entities.OrderBooks;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.EasyBuy.Domain.Tests.Entities
{
    [TestClass]
    public class PriceTests
    {
        private const int PriceAccuracy = 3;
        private const int VolumeAccuracy = 8;

        private OrderBook _orderBook;

        [TestInitialize]
        public void TestInitialize()
        {
            _orderBook = new OrderBook
            {
                Exchange = "Lykke",
                AssetPair = "BTCUSD",
                Timestamp = DateTime.UtcNow,
                SellLevels = new List<OrderBookLevel>(),
                BuyLevels = new List<OrderBookLevel>()
            };
        }

        [TestMethod]
        public void Calculate_Price_By_Four_Levels()
        {
            // arrange

            _orderBook.SellLevels = new List<OrderBookLevel>
            {
                new OrderBookLevel {Price = 5187.4m, Volume = 2.09m},
                new OrderBookLevel {Price = 5187.1m, Volume = 1.66m},
                new OrderBookLevel {Price = 5186.9m, Volume = 1.46m},
                new OrderBookLevel {Price = 5186.5m, Volume = 1.4m},
                new OrderBookLevel {Price = 5185.8m, Volume = 0.4m}
            };

            decimal quoteVolume = 10000;
            decimal markup = 0.05m;

            decimal expectedPrice = 5445.924m;
            decimal expectedVolume = 1.83623569m;

            // act

            Price price = Price.Calculate(_orderBook, quoteVolume, markup, DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(1), PriceAccuracy, VolumeAccuracy);

            // assert

            Assert.IsTrue(price.Value == expectedPrice && price.BaseVolume == expectedVolume);
        }

        [TestMethod]
        public void Calculate_Price_By_One_Level()
        {
            // arrange

            _orderBook.SellLevels = new List<OrderBookLevel>
            {
                new OrderBookLevel {Price = 5186.5m, Volume = 1.4m},
                new OrderBookLevel {Price = 5185.8m, Volume = 10.4m}
            };

            decimal quoteVolume = 10000;
            decimal markup = 0.05m;

            decimal expectedPrice = 5445.09m;
            decimal expectedVolume = 1.83651694m;

            // act

            Price price = Price.Calculate(_orderBook, quoteVolume, markup, DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(1), PriceAccuracy, VolumeAccuracy);

            // assert

            Assert.IsTrue(price.Value == expectedPrice && price.BaseVolume == expectedVolume);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedOperationException), "Quoting volume required.")]
        public void Calculate_With_Wrong_Quoting_Volume()
        {
            // arrange

            _orderBook.SellLevels = new List<OrderBookLevel>
            {
                new OrderBookLevel {Price = 5186.5m, Volume = 1.4m},
                new OrderBookLevel {Price = 5185.8m, Volume = 10.4m}
            };

            decimal quoteVolume = 0;
            decimal markup = 0.05m;

            // act

            Price.Calculate(_orderBook, quoteVolume, markup, DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(1), PriceAccuracy, VolumeAccuracy);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedOperationException), "Empty order book.")]
        public void Calculate_With_Empty_Order_Book()
        {
            // arrange

            decimal quoteVolume = 0;
            decimal markup = 0.05m;

            // act

            Price.Calculate(_orderBook, quoteVolume, markup, DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(1), PriceAccuracy, VolumeAccuracy);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedOperationException), "No liquidity.")]
        public void Calculate_With_No_Enough_Liquidity()
        {
            // arrange

            _orderBook.SellLevels = new List<OrderBookLevel>
            {
                new OrderBookLevel {Price = 5186.9m, Volume = 0.001m},
                new OrderBookLevel {Price = 5186.5m, Volume = 0.001m},
                new OrderBookLevel {Price = 5185.8m, Volume = 0.001m}
            };

            decimal quoteVolume = 10000;
            decimal markup = 0.05m;

            // act

            Price.Calculate(_orderBook, quoteVolume, markup, DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(1), PriceAccuracy, VolumeAccuracy);
        }
    }
}

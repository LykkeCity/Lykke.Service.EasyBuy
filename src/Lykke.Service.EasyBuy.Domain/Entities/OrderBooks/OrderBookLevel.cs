namespace Lykke.Service.EasyBuy.Domain.Entities.OrderBooks
{
    /// <summary>
    /// Represents an order book level.
    /// </summary>
    public class OrderBookLevel
    {
        /// <summary>
        /// The volume of level.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// The price of level.
        /// </summary>
        public decimal Price { get; set; }
    }
}

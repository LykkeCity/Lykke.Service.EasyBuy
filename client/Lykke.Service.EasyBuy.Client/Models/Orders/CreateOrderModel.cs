using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models.Orders
{
    /// <summary>
    /// Represents order creation information.
    /// </summary>
    [PublicAPI]
    public class CreateOrderModel
    {
        /// <summary>
        /// The identifier of client (trading wallet identifier).
        /// </summary>
        public string ClientId;

        /// <summary>
        /// The identifier of price.
        /// </summary>
        public string PriceId;

        /// <summary>
        /// The quoting volume that should be used to create order.
        /// </summary>
        public decimal QuotingVolume;
    }
}

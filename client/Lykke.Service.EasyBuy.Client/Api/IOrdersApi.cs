using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Client.Models;
using Refit;

namespace Lykke.Service.EasyBuy.Client.Api
{
    /// <summary>
    /// Provides methods to work with orders.
    /// </summary>
    public interface IOrdersApi
    {
        /// <summary>
        /// Used to create order.
        /// </summary>
        /// <returns>Order details.</returns>
        [Post("/api/orders")]
        Task<OrderModel> CreateOrderAsync(CreateOrderModel model);

        /// <summary>
        /// Used to fetch existing order.
        /// </summary>
        /// <param name="walletId">Id of the client's wallet.</param>
        /// <param name="id">Id of the order.</param>
        /// <returns>Order details.</returns>
        [Get("/api/orders/{walletId}/{id}")]
        Task<OrderModel> GetOrderAsync(string walletId, string id);
    }
}

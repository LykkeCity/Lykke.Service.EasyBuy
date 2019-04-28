using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.EasyBuy.Client.Models;
using Lykke.Service.EasyBuy.Client.Models.Orders;
using Refit;

namespace Lykke.Service.EasyBuy.Client.Api
{
    /// <summary>
    /// Provides methods to work with orders.
    /// </summary>
    public interface IOrdersApi
    {
        /// <summary>
        /// Returns the order by identifier.
        /// </summary>
        /// <param name="orderId">The identifier of order.</param>
        /// <returns>The order.</returns>
        /// <exception cref="ClientApiException">If order does not exist.</exception>
        [Get("/api/orders/{orderId}")]
        Task<OrderModel> GetByIdAsync(string orderId);

        /// <summary>
        /// Returns orders.
        /// </summary>
        /// <param name="clientId">The identifier for client.</param>
        /// <param name="assetPair">The identifier of asset pair.</param>
        /// <param name="dateFrom">The date from which the orders should be filtered.</param>
        /// <param name="dateTo">The date up to which the orders should be filtered.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to take.</param>
        /// <returns>A collection of orders.</returns>
        [Get("/api/orders")]
        Task<IReadOnlyList<OrderModel>> GetAsync(string clientId, string assetPair, DateTime? dateFrom,
            DateTime? dateTo, int skip, int take);
        
        /// <summary>
        /// Creates new order.
        /// </summary>
        /// <returns>The order.</returns>
        /// <exception cref="ClientApiException">If price not found.</exception>
        /// <exception cref="ClientApiException">If Instrument inactive.</exception>
        /// <exception cref="ClientApiException">If price expired.</exception>
        /// <exception cref="ClientApiException">If quote volume too small.</exception>
        /// <exception cref="ClientApiException">If quote volume too much.</exception>
        [Post("/api/orders")]
        Task<OrderModel> CreateAsync([Body] CreateOrderModel model);

    }
}

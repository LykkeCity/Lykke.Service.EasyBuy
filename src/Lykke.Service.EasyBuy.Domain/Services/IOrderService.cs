using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IOrderService
    {
        Task<Order> GetByIdAsync(string orderId);

        Task<IReadOnlyList<Order>> GetAllAsync(string clientId, string assetPair, DateTime? dateFrom, DateTime? dateTo,
            int skip, int take);

        Task<Order> CreateAsync(string clientId, string priceId, decimal quotingVolume);

        Task ExecuteAsync();
    }
}

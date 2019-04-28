using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;

namespace Lykke.Service.EasyBuy.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(string orderId);

        Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status);

        Task<IReadOnlyList<Order>> GetAsync(string clientId, string assetPair, DateTime? dateFrom, DateTime? dateTo,
            int skip, int take);

        Task InsertAsync(Order order);

        Task UpdateAsync(Order order);
    }
}

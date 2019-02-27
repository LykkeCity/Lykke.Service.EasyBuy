using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Repositories
{
    public interface IOrdersRepository
    {
        Task InsertAsync(Order order);

        Task UpdateAsync(Order order);

        Task<Order> GetAsync(string walletId, string orderId);

        Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status);
    }
}

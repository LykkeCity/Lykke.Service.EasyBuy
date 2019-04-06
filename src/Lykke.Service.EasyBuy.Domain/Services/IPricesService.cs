using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IPricesService
    {
        Task<Price> CreateAsync(string assetPair, OrderType type, decimal quotingVolume, DateTime dateFrom);

        Task<Price> GetAsync(string id);

        Task<IReadOnlyList<Price>> GetActiveAsync(OrderType type);
    }
}

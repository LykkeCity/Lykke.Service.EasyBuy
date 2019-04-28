using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IPriceService
    {
        Task EnsureAsync(string assetPair, DateTime date);

        Task<Price> GetByIdAsync(string priceId);

        Task<Price> GetByAssetPair(string assetPair);

        Task<IReadOnlyList<Price>> GetActiveAsync();

        Task InitializeAsync();
    }
}

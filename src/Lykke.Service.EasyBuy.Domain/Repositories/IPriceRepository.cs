using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;

namespace Lykke.Service.EasyBuy.Domain.Repositories
{
    public interface IPriceRepository
    {
        Task<Price> GetByIdAsync(string priceId);

        Task<IReadOnlyList<Price>> GetLatestAsync();

        Task InsertAsync(Price price);
    }
}

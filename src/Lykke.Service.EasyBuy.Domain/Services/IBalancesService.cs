using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Balances;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IBalancesService
    {
        Task<IReadOnlyList<Balance>> GetAllAsync();

        Task<Balance> GetByAssetAsync(string asset);
    }
}

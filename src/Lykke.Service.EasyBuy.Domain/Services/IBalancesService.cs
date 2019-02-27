using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IBalancesService
    {
        Task<IReadOnlyList<Balance>> GetAsync();
    }
}

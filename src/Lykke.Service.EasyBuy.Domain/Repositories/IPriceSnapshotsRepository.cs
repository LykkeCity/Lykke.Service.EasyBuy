using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Repositories
{
    public interface IPriceSnapshotsRepository
    {   
        Task<PriceSnapshot> GetAsync(string id);
        
        Task InsertAsync(PriceSnapshot priceSnapshot);
    }
}

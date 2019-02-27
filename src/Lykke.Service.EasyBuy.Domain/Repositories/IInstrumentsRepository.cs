using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Repositories
{
    public interface IInstrumentsRepository
    {
        Task<IReadOnlyCollection<Instrument>> GetAllAsync();
        
        Task InsertAsync(Instrument instrument);
        
        Task UpdateAsync(Instrument instrument);
        
        Task DeleteAsync(string assetPairId);
    }
}

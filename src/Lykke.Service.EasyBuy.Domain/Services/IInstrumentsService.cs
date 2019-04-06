using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IInstrumentsService
    {
        Task<IReadOnlyCollection<Instrument>> GetAllAsync();

        Task<Instrument> GetByAssetPairIdAsync(string assetPair);

        Task AddAsync(Instrument instrument);

        Task UpdateAsync(Instrument instrument);

        Task DeleteAsync(string assetPair);
    }
}

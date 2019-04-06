using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IInstrumentsAccessService
    {
        Task<IReadOnlyList<Instrument>> GetAllAsync();

        Task<IReadOnlyList<Instrument>> GetActiveAsync();

        Task<Instrument> GetByAssetPairIdAsync(string assetPair);

        Task AddAsync(Instrument instrument);

        Task<bool> UpdateAsync(Instrument instrument);

        Task DeleteAsync(string assetPair);
    }
}

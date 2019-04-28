using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IInstrumentService
    {
        Task<IReadOnlyList<Instrument>> GetAllAsync();

        Task<IReadOnlyList<Instrument>> GetActiveAsync();

        Task<Instrument> GetByIdAsync(string instrumentId);

        Task<Instrument> GetByAssetPairAsync(string assetPair);

        Task AddAsync(Instrument instrument);

        Task UpdateAsync(Instrument instrument);

        Task DeleteAsync(string instrumentId);
    }
}

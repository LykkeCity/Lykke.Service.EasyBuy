using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;

namespace Lykke.Service.EasyBuy.Domain.Repositories
{
    public interface IInstrumentRepository
    {
        Task<Instrument> GetByIdAsync(string instrumentId);

        Task<IReadOnlyList<Instrument>> GetAllAsync();

        Task InsertAsync(Instrument instrument);

        Task UpdateAsync(Instrument instrument);

        Task DeleteAsync(string instrumentId);
    }
}

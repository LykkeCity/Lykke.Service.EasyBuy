using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class InstrumentsService : IInstrumentsService
    {
        private readonly IInstrumentsAccessService _instrumentsAccessService;

        public InstrumentsService(IInstrumentsAccessService instrumentsAccessService)
        {
            _instrumentsAccessService = instrumentsAccessService;
        }

        public Task<IReadOnlyList<Instrument>> GetAllAsync()
        {
            return _instrumentsAccessService.GetAllAsync();
        }

        public Task<Instrument> GetByAssetPairIdAsync(string assetPair)
        {
            return _instrumentsAccessService.GetByAssetPairIdAsync(assetPair);
        }

        public async Task AddAsync(Instrument instrument)
        {
            await _instrumentsAccessService.AddAsync(instrument);
        }

        public async Task UpdateAsync(Instrument instrument)
        {
            await _instrumentsAccessService.UpdateAsync(instrument);
        }

        public Task DeleteAsync(string assetPair)
        {
            return _instrumentsAccessService.DeleteAsync(assetPair);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class InstrumentsService : IInstrumentsService
    {
        private readonly IInstrumentsRepository _instrumentRepository;
        private readonly ISettingsService _settingsService;
        private readonly InMemoryCache<Instrument> _cache;
        private readonly ILog _log;

        public InstrumentsService(
            IInstrumentsRepository instrumentRepository,
            ISettingsService settingsService,
            ILogFactory logFactory)
        {
            _instrumentRepository = instrumentRepository;
            _settingsService = settingsService;
            _cache = new InMemoryCache<Instrument>(instrument => instrument.AssetPair, false);            
            _log = logFactory.CreateLog(this);
        }

        public async Task<IReadOnlyCollection<Instrument>> GetAllAsync()
        {
            var instruments = _cache.GetAll();

            if (instruments != null)
                return instruments;

            instruments = await _instrumentRepository.GetAllAsync();
                
            _cache.Initialize(instruments);

            return instruments;
        }

        public async Task<Instrument> GetByAssetPairIdAsync(string assetPair)
        {
            var instruments = await GetAllAsync();

            var instrument = instruments.FirstOrDefault(o => o.AssetPair == assetPair);

            if (instrument == null)
                throw new EntityNotFoundException(nameof(assetPair), assetPair);

            return instrument;
        }

        public async Task AddAsync(Instrument instrument)
        {
            IReadOnlyCollection<Instrument> instruments = await GetAllAsync();

            if (instruments.Any(o => o.AssetPair == instrument.AssetPair))
            {
                throw new OperationFailedException("The instrument already used");
            }

            if (!(await _settingsService.GetExternalExchangesAsync()).Contains(instrument.Exchange))
            {
                throw new OperationFailedException("Unknown exchange.");
            }

            await _instrumentRepository.InsertAsync(instrument);

            _cache.Set(instrument);
        }

        public async Task UpdateAsync(Instrument instrument)
        {
            var currentInstrument = await GetByAssetPairIdAsync(instrument.AssetPair);

            currentInstrument.Update(instrument);

            await _instrumentRepository.UpdateAsync(currentInstrument);

            _cache.Set(currentInstrument);
        }

        public async Task DeleteAsync(string assetPairId)
        {
            var instrument = await GetByAssetPairIdAsync(assetPairId);

            if (instrument.State == InstrumentState.Active)
                throw new OperationFailedException("Can not remove active instrument");

            await _instrumentRepository.DeleteAsync(assetPairId);

            _cache.Remove(assetPairId);
        }
    }
}

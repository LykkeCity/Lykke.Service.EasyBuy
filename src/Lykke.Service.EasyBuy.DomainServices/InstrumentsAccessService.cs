using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class InstrumentsAccessService : IInstrumentsAccessService
    {
        private readonly IInstrumentsRepository _instrumentRepository;
        private readonly IOrderBookService _orderBookService;
        private readonly InMemoryCache<Instrument> _cache;

        public InstrumentsAccessService(
            IInstrumentsRepository instrumentRepository,
            IOrderBookService orderBookService)
        {
            _instrumentRepository = instrumentRepository;
            _orderBookService = orderBookService;
            _cache = new InMemoryCache<Instrument>(instrument => instrument.AssetPair, false);
        }

        public async Task<IReadOnlyList<Instrument>> GetAllAsync()
        {
            var instruments = _cache.GetAll();

            if (instruments != null)
                return instruments;

            instruments = await _instrumentRepository.GetAllAsync();
                
            _cache.Initialize(instruments);

            return instruments;
        }

        public async Task<IReadOnlyList<Instrument>> GetActiveAsync()
        {
            var allInstruments = await GetAllAsync();

            var active = allInstruments.Where(x => x.State == InstrumentState.Active).ToList();

            return active;
        }

        public async Task<Instrument> GetByAssetPairIdAsync(string assetPair)
        {
            var instruments = await GetAllAsync();

            var instrument = instruments.FirstOrDefault(o => o.AssetPair == assetPair);

            if (instrument == null)
                throw new EntityNotFoundException();

            return instrument;
        }

        public async Task AddAsync(Instrument instrument)
        {
            var instruments = await GetAllAsync();

            if (instruments.Any(o => o.AssetPair == instrument.AssetPair))
            {
                throw new FailedOperationException("The instrument already used");
            }
            
            if (!_orderBookService.GetExistingExchanges().Contains(instrument.Exchange))
            {
                throw new FailedOperationException("Unknown exchange.");
            }

            await _instrumentRepository.InsertAsync(instrument);

            _cache.Set(instrument);
        }

        public async Task<bool> UpdateAsync(Instrument instrument)
        {
            var currentInstrument = await GetByAssetPairIdAsync(instrument.AssetPair);
            
            if (!_orderBookService.GetExistingExchanges().Contains(instrument.Exchange))
            {
                throw new FailedOperationException($"Unknown '{instrument.Exchange}' exchange.");
            }

            var stateChanged = currentInstrument.State != instrument.State;

            currentInstrument.Update(instrument);

            await _instrumentRepository.UpdateAsync(currentInstrument);

            _cache.Set(currentInstrument);

            return stateChanged;
        }

        public async Task DeleteAsync(string assetPairId)
        {
            var instrument = await GetByAssetPairIdAsync(assetPairId);

            if (instrument.State == InstrumentState.Active)
                throw new FailedOperationException("Can not remove active instrument.");

            await _instrumentRepository.DeleteAsync(assetPairId);

            _cache.Remove(assetPairId);
        }
    }
}

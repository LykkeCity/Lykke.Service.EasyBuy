using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Cache;
using Lykke.Service.EasyBuy.DomainServices.Extensions;

namespace Lykke.Service.EasyBuy.DomainServices
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ILog _log;
        private readonly InMemoryCache<Instrument> _cache;

        public InstrumentService(
            IInstrumentRepository instrumentRepository,
            ILogFactory logFactory)
        {
            _instrumentRepository = instrumentRepository;
            _log = logFactory.CreateLog(this);
            _cache = new InMemoryCache<Instrument>(GetKey, false);
        }

        public async Task<IReadOnlyList<Instrument>> GetAllAsync()
        {
            IReadOnlyList<Instrument> instruments = _cache.GetAll();

            if (instruments == null)
            {
                instruments = await _instrumentRepository.GetAllAsync();

                _cache.Initialize(instruments);
            }

            return instruments;
        }

        public async Task<IReadOnlyList<Instrument>> GetActiveAsync()
        {
            IReadOnlyList<Instrument> instruments = await GetAllAsync();

            return instruments.Where(o => o.Status == InstrumentStatus.Active).ToList();
        }

        public async Task<Instrument> GetByIdAsync(string instrumentId)
        {
            IReadOnlyList<Instrument> instruments = await GetAllAsync();

            return instruments.FirstOrDefault(o => o.Id == instrumentId);
        }

        public async Task<Instrument> GetByAssetPairAsync(string assetPair)
        {
            IReadOnlyList<Instrument> instruments = await GetAllAsync();

            return instruments.FirstOrDefault(o => o.AssetPair == assetPair);
        }

        public async Task AddAsync(Instrument instrument)
        {
            Instrument currentInstrument = await GetByAssetPairAsync(instrument.AssetPair);

            if (currentInstrument != null)
                throw new EntityAlreadyExistsException();

            instrument.Id = Guid.NewGuid().ToString();

            await _instrumentRepository.InsertAsync(instrument);

            _cache.Set(instrument);

            _log.InfoWithDetails("Instrument added", instrument);
        }

        public async Task UpdateAsync(Instrument instrument)
        {
            Instrument currentInstrument = await GetByIdAsync(instrument.Id);

            if (currentInstrument == null)
                throw new EntityNotFoundException();

            await _instrumentRepository.UpdateAsync(instrument);

            _cache.Set(instrument);

            _log.InfoWithDetails("Instrument updated", instrument);
        }

        public async Task DeleteAsync(string instrumentId)
        {
            Instrument instrument = await _instrumentRepository.GetByIdAsync(instrumentId);

            if (instrument == null)
                throw new EntityNotFoundException();

            await _instrumentRepository.DeleteAsync(instrumentId);

            _cache.Remove(GetKey(instrument.AssetPair));

            _log.InfoWithDetails("Instrument deleted", instrument);
        }

        private static string GetKey(Instrument instrument)
            => GetKey(instrument.AssetPair);

        private static string GetKey(string assetPair)
            => assetPair;
    }
}

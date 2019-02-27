using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Repositories;

namespace Lykke.Service.EasyBuy.AzureRepositories.Instruments
{
    public class InstrumentsRepository : IInstrumentsRepository
    {
        private readonly INoSQLTableStorage<InstrumentEntity> _storage;

        public InstrumentsRepository(INoSQLTableStorage<InstrumentEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IReadOnlyCollection<Instrument>> GetAllAsync()
        {
            return Mapper.Map<List<Instrument>>(await _storage.GetDataAsync(GetPartitionKey()));
        }

        public async Task InsertAsync(Instrument instrument)
        {
            var entity = new InstrumentEntity(GetPartitionKey(), GetRowKey(instrument.AssetPair));

            Mapper.Map(instrument, entity);

            await _storage.InsertThrowConflictAsync(entity);
        }

        public async Task UpdateAsync(Instrument instrument)
        {
            await _storage.MergeAsync(GetPartitionKey(), GetRowKey(instrument.AssetPair), entity =>
            {
                Mapper.Map(instrument, entity);
                return entity;
            });
        }

        public Task DeleteAsync(string assetPair)
        {
            return _storage.DeleteAsync(GetPartitionKey(), GetRowKey(assetPair));
        }

        private static string GetPartitionKey()
            => "Instrument";

        private static string GetRowKey(string assetPair)
            => assetPair;
    }
}

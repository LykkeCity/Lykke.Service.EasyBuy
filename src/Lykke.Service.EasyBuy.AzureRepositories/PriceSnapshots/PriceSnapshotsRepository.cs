using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Repositories;

namespace Lykke.Service.EasyBuy.AzureRepositories.PriceSnapshots
{
    public class PriceSnapshotsRepository : IPriceSnapshotsRepository
    {
        private readonly INoSQLTableStorage<PriceSnapshotEntity> _storage;

        public PriceSnapshotsRepository(INoSQLTableStorage<PriceSnapshotEntity> storage)
        {
            _storage = storage;
        }
        
        public async Task<PriceSnapshot> GetAsync(string id)
        {
            return Mapper.Map<PriceSnapshot>(await _storage.GetDataAsync(GetPartitionKey(), GetRowKey(id)));
        }
        
        public async Task InsertAsync(PriceSnapshot priceSnapshot)
        {
            var entity = new PriceSnapshotEntity(GetPartitionKey(), GetRowKey(priceSnapshot.Id));

            Mapper.Map(priceSnapshot, entity);

            await _storage.InsertThrowConflictAsync(entity);
        }
        
        private static string GetPartitionKey()
            => "PriceSnapshot";

        private static string GetRowKey(string id)
            => id;
    }
}

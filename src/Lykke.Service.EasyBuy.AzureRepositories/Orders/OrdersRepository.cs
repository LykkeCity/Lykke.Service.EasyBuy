using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Repositories;

namespace Lykke.Service.EasyBuy.AzureRepositories.Orders
{
    [UsedImplicitly]
    public class OrdersRepository : IOrdersRepository
    {
        private readonly INoSQLTableStorage<OrderEntity> _storage;
        private readonly INoSQLTableStorage<AzureIndex> _indices;

        public OrdersRepository(
            INoSQLTableStorage<OrderEntity> storage,
            INoSQLTableStorage<AzureIndex> indices)
        {
            _storage = storage;
            _indices = indices;
        }
        
        public async Task InsertAsync(Order order)
        {
            var entity = new OrderEntity(GetPartitionKey(order.WalletId), GetRowKey(order.Id));

            Mapper.Map(order, entity);

            await _storage.InsertThrowConflictAsync(entity);
            
            var index = new AzureIndex(GetIndexPartitionKey(order.Status), GetIndexRowKey(order.Id), entity);
            
            await _indices.InsertAsync(index);
        }

        public async Task UpdateAsync(Order order)
        {
            var entity = await _storage.GetDataAsync(GetPartitionKey(order.WalletId), GetRowKey(order.Id));

            OrderStatus? oldStatus = entity.Status != order.Status ? entity.Status : default(OrderStatus?);
            
            Mapper.Map(order, entity);

            await _storage.InsertOrMergeAsync(entity);

            if (oldStatus.HasValue)
            {
                await _indices.DeleteAsync(GetIndexPartitionKey(oldStatus.Value), GetIndexRowKey(order.Id));
                
                var index = new AzureIndex(GetIndexPartitionKey(order.Status), GetIndexRowKey(order.Id), entity);
            
                await _indices.InsertAsync(index);
            }
        }
        
        public async Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status)
        {
            var indices = await _indices.GetDataAsync(GetIndexPartitionKey(status));

            var entities = await _storage.GetDataAsync(indices);

            return Mapper.Map<List<Order>>(entities);
        }

        public async Task<Order> GetAsync(string walletId, string orderId)
        {
            return Mapper.Map<Order>(await _storage.GetDataAsync(GetPartitionKey(walletId), GetRowKey(orderId)));
        }
        
        private static string GetPartitionKey(string walletId)
            => walletId;

        private static string GetRowKey(string id)
            => id;
        
        private static string GetIndexPartitionKey(OrderStatus status)
            => status.ToString();

        private static string GetIndexRowKey(string orderId)
            => orderId;
    }
}

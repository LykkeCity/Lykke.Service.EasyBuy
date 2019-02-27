using System;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.EasyBuy.Domain;

namespace Lykke.Service.EasyBuy.AzureRepositories.Orders
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class OrderEntity : AzureTableEntity
    {
        public OrderEntity()
        {
        }

        public OrderEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public string Id { set; get; }
        
        public string WalletId { set; get; }
        
        public string AssetPair { set; get; }
        
        public OrderType Type { set; get; }
        
        public string PriceId { set; get; }
        
        public decimal BaseVolume { set; get; }
        
        public decimal QuotingVolume { set; get; }
        
        public OrderStatus Status { set; get; }
        
        public DateTime CreatedTime { set; get; }
        
        public string ReserveTransferId { set; get; }
        
        public string SettlementTransferId { set; get; }
    }
}

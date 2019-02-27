using System;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.EasyBuy.Domain;

namespace Lykke.Service.EasyBuy.AzureRepositories.PriceSnapshots
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class PriceSnapshotEntity : AzureTableEntity
    {
        public PriceSnapshotEntity()
        {
        }

        public PriceSnapshotEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public string Id { set; get; }
        
        public string WalletId { set; get; }
        
        public string AssetPair { set; get; }
        
        public OrderType Type { set; get; }
        
        public decimal Price { set; get; }
        
        public decimal Volume { set; get; }
        
        public decimal Markup { set; get; }
        
        public decimal ActualPrice { set; get; }
        
        public string Exchange { set; get; }
        
        public DateTime ValidFrom { set; get; }
        
        public DateTime ValidTo { set; get; }
    }
}

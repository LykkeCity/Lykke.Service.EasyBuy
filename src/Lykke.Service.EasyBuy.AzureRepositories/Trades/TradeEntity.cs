using System;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.EasyBuy.Domain;

namespace Lykke.Service.EasyBuy.AzureRepositories.Trades
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class TradeEntity : AzureTableEntity
    {
        public TradeEntity()
        {
        }

        public TradeEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public string Id { set; get; }
        
        public string WalletId { set; get; }
        
        public string OrderId { set; get; }
        
        public OrderType Type { set; get; }
        
        public decimal BaseVolume { set; get; }
        
        public decimal QuotingVolume { set; get; }
        
        public DateTime DateTime { set; get; }
    }
}

using System;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.EasyBuy.Domain;

namespace Lykke.Service.EasyBuy.AzureRepositories.Instruments
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class InstrumentEntity : AzureTableEntity
    {
        public InstrumentEntity()
        {
        }

        public InstrumentEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        
        public string AssetPair { set; get; }
        
        public string Exchange { set; get; }
        
        public TimeSpan? PriceLifetime { set; get; }
        
        public decimal? Markup { set; get; }
        
        public decimal Volume { set; get; }
        
        public InstrumentState State { get; set; }
    }
}

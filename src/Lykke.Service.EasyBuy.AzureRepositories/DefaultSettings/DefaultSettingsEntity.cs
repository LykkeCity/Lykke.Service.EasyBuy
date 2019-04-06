using System;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Service.EasyBuy.AzureRepositories.DefaultSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class DefaultSettingsEntity : AzureTableEntity
    {
        public DefaultSettingsEntity()
        {
        }

        public DefaultSettingsEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public decimal? Markup { set; get; }
        
        public TimeSpan? RecalculationInterval { set; get; }

        public TimeSpan? OverlapTime { set; get; }

        public TimeSpan? PriceLifetime { set; get; }

        public TimeSpan? OrdersProcessorPeriod { set; get; }

        public TimeSpan? PriceGeneratorPeriod { set; get; }
    }
}

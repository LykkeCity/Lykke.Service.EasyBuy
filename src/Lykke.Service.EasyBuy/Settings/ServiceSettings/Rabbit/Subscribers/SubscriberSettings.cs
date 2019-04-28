using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Subscribers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SubscriberSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        [Optional]
        [AmqpCheck]
        public string AlternateConnectionString { get; set; }

        public string Exchange { get; set; }

        public string QueueSuffix { get; set; }
    }
}

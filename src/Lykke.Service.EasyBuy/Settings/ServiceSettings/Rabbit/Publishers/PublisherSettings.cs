using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Publishers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PublisherSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string Exchange { get; set; }
    }
}

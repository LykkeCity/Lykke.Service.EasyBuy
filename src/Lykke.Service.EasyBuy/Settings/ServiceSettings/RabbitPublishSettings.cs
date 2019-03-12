using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitPublishSettings
    {
        [AmqpCheck]
        public string ConnectionString { set; get; }
        
        public string ExchangeName { set; get; }
    }
}

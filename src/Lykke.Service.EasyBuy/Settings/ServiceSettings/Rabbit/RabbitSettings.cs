using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Publishers;
using Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Subscribers;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSettings
    {
        public RabbitSubscribers Subscribers { get; set; }

        public RabbitPublishers Publishers { get; set; }
    }
}

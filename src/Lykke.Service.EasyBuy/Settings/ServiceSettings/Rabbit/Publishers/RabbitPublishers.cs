using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Publishers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitPublishers
    {
        public PublisherSettings Prices { get; set; }
    }
}

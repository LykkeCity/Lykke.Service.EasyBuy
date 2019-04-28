using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Subscribers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSubscribers
    {
        public MultiSourceSettings OrderBooks { get; set; }
    }
}

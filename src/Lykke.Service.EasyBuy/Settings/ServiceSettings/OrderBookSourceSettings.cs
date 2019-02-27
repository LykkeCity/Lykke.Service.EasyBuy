using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class OrderBookSourceSettings
    {
        public string Name { set; get; }
        
        public RabbitSettings Rabbit { set; get; }
    }
}

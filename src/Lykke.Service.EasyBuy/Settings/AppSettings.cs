using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.Assets.Client;
using Lykke.Service.Balances.Client;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.EasyBuy.Settings.ServiceSettings;
using Lykke.Service.ExchangeOperations.Client;

namespace Lykke.Service.EasyBuy.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public EasyBuySettings EasyBuyService { get; set; }
        
        public BalancesServiceClientSettings BalancesServiceClient { get; set; }

        public ExchangeOperationsServiceClientSettings ExchangeOperationsServiceClient { get; set; }
        
        public AssetsServiceClientSettings AssetsServiceClient { get; set; }
        
        public ClientAccountServiceClientSettings ClientAccountServiceClient { get; set; } 
    }
}

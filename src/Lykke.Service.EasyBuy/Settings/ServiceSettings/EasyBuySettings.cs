using System;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Settings.ServiceSettings.Db;
using Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EasyBuySettings
    {
        public string InstanceName { set; get; }

        public string WalletId { set; get; }

        public TimeSpan RecalculationInterval { get; set; }

        public TimeSpan OrderExecutionInterval { get; set; }

        public DbSettings Db { get; set; }

        public RabbitSettings Rabbit { set; get; }
    }
}

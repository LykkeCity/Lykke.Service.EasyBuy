using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Settings.ServiceSettings.Db;

namespace Lykke.Service.EasyBuy.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EasyBuySettings
    {
        public string InstanceName { set; get; }
        
        public string ClientId { set; get; }
        
        public decimal DefaultMarkup { set; get; }
        
        public TimeSpan TimerPeriod { set; get; }
        
        public TimeSpan DefaultPriceLifetime { set; get; }
        
        public DbSettings Db { get; set; }
        
        public OrderBookSourceSettings[] OrderBookSources { set; get; }
    }
}

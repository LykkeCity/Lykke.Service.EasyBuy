using System;
using JetBrains.Annotations;

namespace Lykke.Service.EasyBuy.Client.Models
{
    [PublicAPI]
    public class OrderModel
    {
        public string Id { set; get; }
        
        public string WalletId { set; get; }
        
        public string AssetPair { set; get; }
        
        public OrderType Type { set; get; }
        
        public string PriceId { set; get; }
        
        public decimal Volume { set; get; }
        
        public DateTime CreatedTime { set; get; }
    }
}

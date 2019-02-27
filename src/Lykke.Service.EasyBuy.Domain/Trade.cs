using System;

namespace Lykke.Service.EasyBuy.Domain
{
    public class Trade
    {
        public string Id { set; get; }
        
        public string WalletId { set; get; }
        
        public string OrderId { set; get; }
        
        public OrderType Type { set; get; }
        
        public decimal BaseVolume { set; get; }
        
        public decimal QuotingVolume { set; get; }
        
        public DateTime Timestamp { set; get; }
    }
}

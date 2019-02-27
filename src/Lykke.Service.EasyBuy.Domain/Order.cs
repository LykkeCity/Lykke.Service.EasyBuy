using System;

namespace Lykke.Service.EasyBuy.Domain
{
    public class Order
    {
        public string Id { set; get; }
        
        public string WalletId { set; get; }
        
        public string AssetPair { set; get; }
        
        public OrderType Type { set; get; }
        
        public string PriceId { set; get; }
        
        public decimal BaseVolume { set; get; }
        
        public decimal QuotingVolume { set; get; }
        
        public OrderStatus Status { set; get; }
        
        public DateTime CreatedTime { set; get; }
        
        public string ReserveTransferId { set; get; }
        
        public string SettlementTransferId { set; get; }
    }
}

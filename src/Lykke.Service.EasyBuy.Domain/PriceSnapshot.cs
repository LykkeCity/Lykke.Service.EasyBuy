using System;

namespace Lykke.Service.EasyBuy.Domain
{
    public class PriceSnapshot
    {
        public string Id { set; get; }
        
        public string WalletId { set; get; }
        
        public string AssetPair { set; get; }
        
        public OrderType Type { set; get; }
        
        public decimal Price { set; get; }
        
        public decimal BaseVolume { set; get; }
        
        public decimal QuotingVolume { set; get; }
        
        public decimal Markup { set; get; }
        
        public decimal OriginalPrice { set; get; }
        
        public string Exchange { set; get; }
        
        public DateTime ValidFrom { set; get; }
        
        public DateTime ValidTo { set; get; }
    }
}

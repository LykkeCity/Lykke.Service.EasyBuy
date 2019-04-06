using System;

namespace Lykke.Service.EasyBuy.Domain
{
    public class Price
    {
        public string Id { set; get; }
        
        public string AssetPair { set; get; }
        
        public OrderType Type { set; get; }
        
        /// <summary>
        /// Final price with markup
        /// </summary>
        public decimal Value { set; get; }
        
        public decimal BaseVolume { set; get; }
        
        public decimal QuotingVolume { set; get; }
        
        public decimal Markup { set; get; }
        
        public decimal OriginalPrice { set; get; }
        
        public string Exchange { set; get; }
        
        public DateTime ValidFrom { set; get; }
        
        public DateTime ValidTo { set; get; }
        
        public TimeSpan AllowedOverlap { set; get; }
    }
}

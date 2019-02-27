namespace Lykke.Service.EasyBuy.Domain
{
    public class Balance
    {
        public string AssetId { set; get; }
        
        public decimal Available { set; get; }
        
        public decimal Reserved { set; get; }
    }
}

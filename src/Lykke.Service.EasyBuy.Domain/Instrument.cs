using System;

namespace Lykke.Service.EasyBuy.Domain
{
    public class Instrument
    {
        public string AssetPair { set; get; }
        
        public string Exchange { set; get; }
        
        public TimeSpan? PriceLifetime { set; get; }
        
        /// <summary>
        /// How long a price works after lifetime ends
        /// </summary>
        public TimeSpan? OverlapTime { set; get; }
        
        public TimeSpan? RecalculationInterval { set; get; }
        
        public decimal? Markup { set; get; }
        
        public decimal Volume { set; get; }
        
        public InstrumentState State { get; set; }

        public void Update(Instrument instrument)
        {
            Exchange = instrument.Exchange;
            PriceLifetime = instrument.PriceLifetime;
            OverlapTime = instrument.OverlapTime;
            RecalculationInterval = instrument.RecalculationInterval;
            Volume = instrument.Volume;
            Markup = instrument.Markup;
            State = instrument.State;
        }
    }
}

using System;

namespace Lykke.Service.EasyBuy.Domain
{
    public class Instrument
    {
        public string AssetPair { set; get; }
        
        public string Exchange { set; get; }
        
        public TimeSpan? PriceLifetime { set; get; }
        
        public decimal? Markup { set; get; }
        
        public InstrumentState State { get; set; }

        public void Update(Instrument instrument)
        {
            Exchange = instrument.Exchange;
            PriceLifetime = instrument.PriceLifetime;
            Markup = instrument.Markup;
            State = instrument.State;
        }
    }
}

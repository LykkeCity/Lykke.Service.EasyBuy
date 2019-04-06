using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;
using Lykke.Service.EasyBuy.Domain;

namespace Lykke.Service.EasyBuy
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Instrument, InstrumentModel>(MemberList.Source);
            CreateMap<InstrumentModel, Instrument>(MemberList.Destination);
            
            CreateMap<Order, OrderModel>(MemberList.Destination)
                .ForMember(o => o.Volume, opt => opt.MapFrom(x => x.BaseVolume));
            
            CreateMap<Price, PriceModel>(MemberList.Destination);

            CreateMap<Common.ExchangeAdapter.Contracts.OrderBookItem, OrderBookLimitOrder>(MemberList.Destination);
            CreateMap<Common.ExchangeAdapter.Contracts.OrderBook, OrderBook>(MemberList.Destination)
                .ForMember(o => o.AssetPair, opt => opt.MapFrom(x => x.Asset))
                .ForMember(o => o.SellLimitOrders, opt => opt.MapFrom(x => x.Asks))
                .ForMember(o => o.BuyLimitOrders, opt => opt.MapFrom(x => x.Bids));
        }
    }
}

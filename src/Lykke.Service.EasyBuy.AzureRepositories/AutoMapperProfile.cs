using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.AzureRepositories.Instruments;
using Lykke.Service.EasyBuy.AzureRepositories.Orders;
using Lykke.Service.EasyBuy.AzureRepositories.PriceSnapshots;
using Lykke.Service.EasyBuy.AzureRepositories.Trades;
using Lykke.Service.EasyBuy.Domain;

namespace Lykke.Service.EasyBuy.AzureRepositories
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Instrument, InstrumentEntity>(MemberList.Source);
            CreateMap<InstrumentEntity, Instrument>(MemberList.Destination);
            
            CreateMap<PriceSnapshot, PriceSnapshotEntity>(MemberList.Source);
            CreateMap<PriceSnapshotEntity, PriceSnapshot>(MemberList.Destination);
            
            CreateMap<Trade, TradeEntity>(MemberList.Source);
            CreateMap<TradeEntity, Trade>(MemberList.Destination);
            
            CreateMap<Order, OrderEntity>(MemberList.Source);
            CreateMap<OrderEntity, Order>(MemberList.Destination);
        }
    }
}

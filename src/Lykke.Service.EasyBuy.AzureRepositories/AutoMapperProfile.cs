using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.AzureRepositories.DefaultSettings;
using Lykke.Service.EasyBuy.AzureRepositories.Instruments;
using Lykke.Service.EasyBuy.AzureRepositories.Orders;
using Lykke.Service.EasyBuy.AzureRepositories.Prices;
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
            
            CreateMap<Price, PriceEntity>(MemberList.Source);
            CreateMap<PriceEntity, Price>(MemberList.Destination);
            
            CreateMap<Trade, TradeEntity>(MemberList.Source);
            CreateMap<TradeEntity, Trade>(MemberList.Destination);
            
            CreateMap<Order, OrderEntity>(MemberList.Source);
            CreateMap<OrderEntity, Order>(MemberList.Destination);
            
            CreateMap<DefaultSetting, DefaultSettingsEntity>(MemberList.Source);
            CreateMap<DefaultSettingsEntity, DefaultSetting>(MemberList.Destination);
        }
    }
}

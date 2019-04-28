using System;
using AutoMapper;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;
using Lykke.Service.EasyBuy.PostgresRepositories.Entities;

namespace Lykke.Service.EasyBuy.PostgresRepositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Instrument, InstrumentEntity>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
            CreateMap<InstrumentEntity, Instrument>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<Order, OrderEntity>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => Guid.Parse(src.ClientId)))
                .ForMember(dest => dest.PriceId, opt => opt.MapFrom(src => Guid.Parse(src.PriceId)));
            CreateMap<OrderEntity, Order>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId.ToString()))
                .ForMember(dest => dest.PriceId, opt => opt.MapFrom(src => src.PriceId.ToString()));

            CreateMap<Price, PriceEntity>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
            CreateMap<PriceEntity, Price>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<Transfer, TransferEntity>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => Guid.Parse(src.OrderId)));
            CreateMap<TransferEntity, Transfer>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId.ToString()));
        }
    }
}

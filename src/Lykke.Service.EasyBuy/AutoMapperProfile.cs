using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models.Balances;
using Lykke.Service.EasyBuy.Client.Models.Instruments;
using Lykke.Service.EasyBuy.Client.Models.Orders;
using Lykke.Service.EasyBuy.Client.Models.Prices;
using Lykke.Service.EasyBuy.Domain.Entities.Balances;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;

namespace Lykke.Service.EasyBuy
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Balance, BalanceModel>(MemberList.Source);

            CreateMap<Instrument, InstrumentModel>(MemberList.Source);
            CreateMap<InstrumentModel, Instrument>(MemberList.Destination);

            CreateMap<Order, OrderModel>(MemberList.Source);

            CreateMap<Price, PriceModel>(MemberList.Source);

            CreateMap<Price, Contract.Price>(MemberList.Source);
        }
    }
}

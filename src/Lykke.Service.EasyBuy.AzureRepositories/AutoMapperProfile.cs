using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.AzureRepositories.Instruments;
using Lykke.Service.EasyBuy.AzureRepositories.PriceSnapshots;
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
        }
    }
}

using Autofac;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Timers;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        public AutofacModule()
        {
            
        }

        protected override void Load(ContainerBuilder builder)
        {
            LoadServices(builder);

            LoadTimers(builder);
        }

        private void LoadServices(ContainerBuilder builder)
        {
            builder.RegisterType<OrderBookService>()
                .As<IOrderBookService>()
                .SingleInstance();

            builder.RegisterType<InstrumentsService>()
                .As<IInstrumentsService>()
                .SingleInstance();

            builder.RegisterType<OrdersService>()
                .As<IOrdersService>()
                .SingleInstance();

            builder.RegisterType<BalanceService>()
                .As<IBalancesService>()
                .SingleInstance();
        }
        
        private void LoadTimers(ContainerBuilder builder)
        {
            builder.RegisterType<OrdersProcessorTimer>()
                .AsSelf()
                .SingleInstance();
        }
    }
}

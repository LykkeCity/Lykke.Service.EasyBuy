using Autofac;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Timers;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly string _instanceName;
        private readonly string _walletId;
        
        public AutofacModule(
            string instanceName,
            string walletId)
        {
            _instanceName = instanceName;
            _walletId = walletId;
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

            builder.RegisterType<InstrumentsAccessService>()
                .As<IInstrumentsAccessService>()
                .SingleInstance();

            builder.RegisterType<InstrumentsService>()
                .As<IInstrumentsService>()
                .SingleInstance();

            builder.RegisterType<PricesService>()
                .As<IPricesService>()
                .SingleInstance();

            builder.RegisterType<InternalTransfersService>()
                .As<IInternalTransfersService>()
                .SingleInstance();

            builder.RegisterType<OrdersService>()
                .As<IOrdersService>()
                .SingleInstance();

            builder.RegisterType<BalanceService>()
                .As<IBalancesService>()
                .SingleInstance();

            builder.RegisterType<PricesGenerator>()
                .As<IPricesGenerator>()
                .SingleInstance();

            builder.RegisterType<SettingsService>()
                .WithParameter("instanceName", _instanceName)
                .WithParameter("walletId", _walletId)
                .As<ISettingsService>()
                .SingleInstance();
        }
        
        private void LoadTimers(ContainerBuilder builder)
        {
            builder.RegisterType<OrdersProcessorTimer>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<PricesGeneratorTimer>()
                .AsSelf()
                .SingleInstance();
        }
    }
}

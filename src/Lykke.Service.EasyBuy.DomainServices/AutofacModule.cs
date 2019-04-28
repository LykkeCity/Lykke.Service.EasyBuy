using System;
using Autofac;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Domain.Services;

namespace Lykke.Service.EasyBuy.DomainServices
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly string _instanceName;
        private readonly string _walletId;
        private readonly TimeSpan _recalculationInterval;
        private readonly TimeSpan _orderExecutionInterval;

        public AutofacModule(
            string instanceName,
            string walletId,
            TimeSpan recalculationInterval,
            TimeSpan orderExecutionInterval)
        {
            _instanceName = instanceName;
            _walletId = walletId;
            _recalculationInterval = recalculationInterval;
            _orderExecutionInterval = orderExecutionInterval;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BalanceService>()
                .As<IBalancesService>()
                .SingleInstance();

            builder.RegisterType<ExchangeService>()
                .As<IExchangeService>()
                .SingleInstance();

            builder.RegisterType<InstrumentService>()
                .As<IInstrumentService>()
                .SingleInstance();

            builder.RegisterType<OrderBookService>()
                .As<IOrderBookService>()
                .SingleInstance();

            builder.RegisterType<OrderService>()
                .As<IOrderService>()
                .SingleInstance();

            builder.RegisterType<PriceService>()
                .As<IPriceService>()
                .SingleInstance();

            builder.RegisterType<SettingsService>()
                .WithParameter("instanceName", _instanceName)
                .WithParameter("walletId", _walletId)
                .WithParameter("recalculationInterval", _recalculationInterval)
                .WithParameter("orderExecutionInterval", _orderExecutionInterval)
                .As<ISettingsService>()
                .SingleInstance();
        }
    }
}

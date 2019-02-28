using System;
using System.Collections.Generic;
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
        private readonly string _instanceName;
        private readonly string _clientId;
        private readonly decimal _defaultMarkup;
        private readonly TimeSpan _defaultPriceLifetime;
        private readonly TimeSpan _timerPeriod;
        private readonly IEnumerable<string> _externalExchanges;
        
        public AutofacModule(
            string instanceName,
            string clientId,
            decimal defaultMarkup,
            TimeSpan timerPeriod,
            TimeSpan defaultPriceLifetime,
            IEnumerable<string> externalExchanges)
        {
            _instanceName = instanceName;
            _clientId = clientId;
            _defaultMarkup = defaultMarkup;
            _timerPeriod = timerPeriod;
            _defaultPriceLifetime = defaultPriceLifetime;
            _externalExchanges = externalExchanges;
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

            builder.Register(x => new SettingsService(
                    _instanceName,
                    _clientId,
                    _defaultMarkup,
                    _timerPeriod,
                    _defaultPriceLifetime,
                    _externalExchanges))
                .As<ISettingsService>()
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

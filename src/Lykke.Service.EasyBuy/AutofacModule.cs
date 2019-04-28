using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.Assets.Client;
using Lykke.Service.Balances.Client;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.Managers;
using Lykke.Service.EasyBuy.Rabbit.Publishers;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;
using Lykke.Service.EasyBuy.Settings;
using Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Subscribers;
using Lykke.Service.EasyBuy.Timers;
using Lykke.Service.ExchangeOperations.Client;
using Lykke.SettingsReader;

namespace Lykke.Service.EasyBuy
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public AutofacModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DomainServices.AutofacModule(
                _settings.CurrentValue.EasyBuyService.InstanceName,
                _settings.CurrentValue.EasyBuyService.WalletId,
                _settings.CurrentValue.EasyBuyService.RecalculationInterval,
                _settings.CurrentValue.EasyBuyService.OrderExecutionInterval));

            builder.RegisterModule(
                new PostgresRepositories.AutofacModule(_settings.CurrentValue.EasyBuyService.Db.DataConnectionString));

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            RegisterRabbit(builder);

            RegisterClients(builder);

            RegisterTimers(builder);
        }

        private void RegisterRabbit(ContainerBuilder builder)
        {
            MultiSourceSettings orderBookSubscribers =
                _settings.CurrentValue.EasyBuyService.Rabbit.Subscribers.OrderBooks;

            foreach (string exchange in orderBookSubscribers.Exchanges)
            {
                builder.RegisterType<OrderBookSubscriber>()
                    .WithParameter(TypedParameter.From(new SubscriberSettings
                    {
                        Exchange = exchange,
                        QueueSuffix = orderBookSubscribers.QueueSuffix,
                        ConnectionString = orderBookSubscribers.ConnectionString
                    }))
                    .AsSelf()
                    .SingleInstance();
            }

            builder.RegisterType<PricesPublisher>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.EasyBuyService.Rabbit.Publishers.Prices))
                .AsSelf()
                .As<IPricesPublisher>()
                .SingleInstance();
        }

        private void RegisterClients(ContainerBuilder builder)
        {
            builder.RegisterAssetsClient(_settings.CurrentValue.AssetsServiceClient);

            builder.RegisterBalancesClient(_settings.CurrentValue.BalancesServiceClient);

            builder.RegisterExchangeOperationsClient(_settings.CurrentValue.ExchangeOperationsServiceClient.ServiceUrl);
        }

        private static void RegisterTimers(ContainerBuilder builder)
        {
            builder.RegisterType<OrdersTimer>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<PricesTimer>()
                .AsSelf()
                .SingleInstance();
        }
    }
}

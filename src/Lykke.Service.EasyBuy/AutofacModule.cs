using System;
using System.Linq;
using System.Net;
using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.Assets.Client;
using Lykke.Service.Balances.Client;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.Managers;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;
using Lykke.Service.EasyBuy.Settings;
using Lykke.Service.EasyBuy.Settings.ServiceSettings;
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
                _settings.CurrentValue.EasyBuyService.ClientId,
                _settings.CurrentValue.EasyBuyService.DefaultMarkup,
                _settings.CurrentValue.EasyBuyService.TimerPeriod,
                _settings.CurrentValue.EasyBuyService.DefaultPriceLifetime,
                _settings.CurrentValue.EasyBuyService.OrderBookSources.Select(x => x.Name)));
            
            builder.RegisterModule(new AzureRepositories.AutofacModule(_settings.Nested(o =>
                o.EasyBuyService.Db.DataConnectionString)));
            
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();
            
            RegisterRabbit(builder);

            RegisterClients(builder);
        }

        private void RegisterRabbit(ContainerBuilder builder)
        {
            foreach (var source in _settings.CurrentValue.EasyBuyService.OrderBookSources)
            {
                builder.RegisterType<OrderBookSubscriber>()
                    .AsSelf()
                    .WithParameter(TypedParameter.From(source.Rabbit))
                    .WithParameter(TypedParameter.From(source.Name))
                    .Named<OrderBookSubscriber>(source.Name)
                    .SingleInstance();
            }
        }
        
        private void RegisterClients(ContainerBuilder builder)
        {
            builder.RegisterAssetsClient(new AssetServiceSettings
            {
                BaseUri = new Uri(_settings.CurrentValue.AssetsServiceClient.ServiceUrl),
                AssetsCacheExpirationPeriod = TimeSpan.FromHours(1),
                AssetPairsCacheExpirationPeriod = TimeSpan.FromHours(1)
            });
            
            builder.RegisterLykkeServiceClient(_settings.CurrentValue.ClientAccountServiceClient.ServiceUrl);

            builder.RegisterBalancesClient(_settings.CurrentValue.BalancesServiceClient);

            builder.RegisterExchangeOperationsClient(_settings.CurrentValue.ExchangeOperationsServiceClient.ServiceUrl);
        }
    }
}

using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.Managers;
using Lykke.Service.EasyBuy.Rabbit.Subscribers;
using Lykke.Service.EasyBuy.Settings;
using Lykke.Service.EasyBuy.Settings.ServiceSettings;
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
            builder.RegisterModule(new DomainServices.AutofacModule());
            builder.RegisterModule(new AzureRepositories.AutofacModule(_settings.Nested(o =>
                o.EasyBuyService.Db.DataConnectionString)));
            
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();
            
            RegisterRabbit(builder);
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
    }
}

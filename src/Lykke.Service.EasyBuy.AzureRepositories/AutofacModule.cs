using Autofac;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.AzureRepositories.Instruments;
using Lykke.Service.EasyBuy.AzureRepositories.Orders;
using Lykke.Service.EasyBuy.AzureRepositories.PriceSnapshots;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.SettingsReader;

namespace Lykke.Service.EasyBuy.AzureRepositories
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<string> _connectionString;

        public AutofacModule(IReloadingManager<string> connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(container => new InstrumentsRepository(
                    AzureTableStorage<InstrumentEntity>.Create(_connectionString,
                        "Instruments", container.Resolve<ILogFactory>())))
                .As<IInstrumentsRepository>()
                .SingleInstance();

            builder.Register(container => new PriceSnapshotsRepository(
                    AzureTableStorage<PriceSnapshotEntity>.Create(_connectionString,
                        "PriceSnapshots", container.Resolve<ILogFactory>())))
                .As<IPriceSnapshotsRepository>()
                .SingleInstance();

            builder.Register(container => new OrdersRepository(
                    AzureTableStorage<OrderEntity>.Create(_connectionString,
                        "Orders", container.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(_connectionString,
                        "Orders", container.Resolve<ILogFactory>())))
                .As<IOrdersRepository>()
                .SingleInstance();
        }
    }
}

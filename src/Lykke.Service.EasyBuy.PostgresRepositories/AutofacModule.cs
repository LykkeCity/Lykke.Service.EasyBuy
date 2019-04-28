using Autofac;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.PostgresRepositories.Repositories;

namespace Lykke.Service.EasyBuy.PostgresRepositories
{
    public class AutofacModule : Module
    {
        private readonly string _connectionString;

        public AutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionFactory>()
                .AsSelf()
                .WithParameter(TypedParameter.From(_connectionString))
                .SingleInstance();

            builder.RegisterType<InstrumentRepository>()
                .As<IInstrumentRepository>()
                .SingleInstance();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .SingleInstance();

            builder.RegisterType<PriceRepository>()
                .As<IPriceRepository>()
                .SingleInstance();

            builder.RegisterType<TransferRepository>()
                .As<ITransferRepository>()
                .SingleInstance();
        }
    }
}

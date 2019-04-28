using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;
using Lykke.Service.EasyBuy.PostgresRepositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lykke.Service.EasyBuy.PostgresRepositories
{
    public class DataContext : DbContext
    {
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal DbSet<InstrumentEntity> Instruments { get; set; }

        internal DbSet<OrderEntity> Orders { get; set; }

        internal DbSet<PriceEntity> Prices { get; set; }

        internal DbSet<TransferEntity> Transfers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Positions

            modelBuilder.Entity<InstrumentEntity>()
                .Property(o => o.Status)
                .HasConversion(new EnumToNumberConverter<InstrumentStatus, short>());

            // Orders

            modelBuilder.Entity<OrderEntity>()
                .Property(o => o.Status)
                .HasConversion(new EnumToNumberConverter<OrderStatus, short>());

            // Transfers

            modelBuilder.Entity<TransferEntity>()
                .Property(o => o.Type)
                .HasConversion(new EnumToNumberConverter<TransferType, short>());
        }
    }
}

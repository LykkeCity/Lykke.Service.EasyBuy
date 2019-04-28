using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.PostgresRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Service.EasyBuy.PostgresRepositories.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private const string LatestPricesView = "latest_prices";

        private readonly ConnectionFactory _connectionFactory;

        public PriceRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Price> GetByIdAsync(string priceId)
        {
            if (!Guid.TryParse(priceId, out Guid id))
                throw new FailedOperationException("Invalid price identifier");

            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                PriceEntity entity = await context.Prices
                    .SingleOrDefaultAsync(o => o.Id == id);

                return Mapper.Map<Price>(entity);
            }
        }

        public async Task<IReadOnlyList<Price>> GetLatestAsync()
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                string command = $"SELECT * FROM {LatestPricesView}";

                List<PriceEntity> entities = await context.Prices
                    .FromSql(command)
                    .ToListAsync();

                return Mapper.Map<List<Price>>(entities);
            }
        }

        public async Task InsertAsync(Price price)
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                var entity = Mapper.Map<PriceEntity>(price);

                context.Prices.Add(entity);

                await context.SaveChangesAsync();
            }
        }
    }
}

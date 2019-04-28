using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.PostgresRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Service.EasyBuy.PostgresRepositories.Repositories
{
    public class InstrumentRepository : IInstrumentRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public InstrumentRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Instrument> GetByIdAsync(string instrumentId)
        {
            if (!Guid.TryParse(instrumentId, out Guid id))
                throw new FailedOperationException("Invalid instrument identifier");

            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                InstrumentEntity entity = await context.Instruments
                    .SingleOrDefaultAsync(o => o.Id == id);

                return Mapper.Map<Instrument>(entity);
            }
        }

        public async Task<IReadOnlyList<Instrument>> GetAllAsync()
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                List<InstrumentEntity> entities = await context.Instruments.ToListAsync();

                return Mapper.Map<List<Instrument>>(entities);
            }
        }

        public async Task InsertAsync(Instrument instrument)
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                var entity = Mapper.Map<InstrumentEntity>(instrument);

                context.Instruments.Add(entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Instrument instrument)
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                var entity = Mapper.Map<InstrumentEntity>(instrument);

                context.Instruments.Update(entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string instrumentId)
        {
            if (!Guid.TryParse(instrumentId, out Guid id))
                throw new ArgumentException("Invalid identifier", nameof(instrumentId));

            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                var entity = new InstrumentEntity {Id = id};

                context.Instruments.Attach(entity);

                context.Instruments.Remove(entity);

                await context.SaveChangesAsync();
            }
        }
    }
}

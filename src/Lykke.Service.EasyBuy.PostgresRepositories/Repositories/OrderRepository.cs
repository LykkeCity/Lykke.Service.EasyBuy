using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Lykke.Service.EasyBuy.PostgresRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Service.EasyBuy.PostgresRepositories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public OrderRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Order> GetByIdAsync(string orderId)
        {
            if (!Guid.TryParse(orderId, out Guid id))
                throw new FailedOperationException("Invalid order identifier");

            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                OrderEntity entity = await context.Orders
                    .SingleOrDefaultAsync(o => o.Id == id);

                return Mapper.Map<Order>(entity);
            }
        }

        public async Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status)
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                List<OrderEntity> entities = await context.Orders
                    .Where(o => o.Status == status)
                    .ToListAsync();

                return Mapper.Map<List<Order>>(entities);
            }
        }

        public async Task<IReadOnlyList<Order>> GetAsync(string clientId, string assetPair, DateTime? dateFrom,
            DateTime? dateTo, int skip, int take)
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                IQueryable<OrderEntity> query = context.Orders.AsQueryable();

                if (Guid.TryParse(clientId, out Guid id))
                    query = query.Where(o => o.ClientId == id);

                if (!string.IsNullOrEmpty(assetPair))
                    query = query.Where(o => o.AssetPair == assetPair);

                if (dateFrom.HasValue)
                    query = query.Where(o => o.CreatedDate >= dateFrom.Value);

                if (dateTo.HasValue)
                    query = query.Where(o => o.CreatedDate <= dateTo.Value);

                List<OrderEntity> entities = await query.Skip(skip).Take(take).ToListAsync();

                return Mapper.Map<List<Order>>(entities);
            }
        }

        public async Task InsertAsync(Order order)
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                var entity = Mapper.Map<OrderEntity>(order);

                context.Orders.Add(entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Order order)
        {
            using (DataContext context = _connectionFactory.CreateDataContext())
            {
                var entity = Mapper.Map<OrderEntity>(order);

                context.Orders.Update(entity);

                await context.SaveChangesAsync();
            }
        }
    }
}

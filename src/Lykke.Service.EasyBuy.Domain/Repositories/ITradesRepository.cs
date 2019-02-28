using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Repositories
{
    public interface ITradesRepository
    {
        Task<IReadOnlyList<Trade>> GetAsync(DateTime from, DateTime to);

        Task InsertAsync(Trade trade);
    }
}

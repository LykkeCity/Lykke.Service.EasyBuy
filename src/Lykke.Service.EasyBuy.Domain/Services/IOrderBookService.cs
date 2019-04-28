using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.OrderBooks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IOrderBookService
    {
        OrderBook GetByAssetPair(string exchange, string assetPair);

        IReadOnlyList<string> GetExistingExchanges();

        Task HandleAsync(OrderBook orderBook);
    }
}

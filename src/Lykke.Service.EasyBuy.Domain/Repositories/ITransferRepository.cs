using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;

namespace Lykke.Service.EasyBuy.Domain.Repositories
{
    public interface ITransferRepository
    {
        Task<Transfer> GetAsync(string orderId, TransferType type);
        
        Task InsertAsync(Transfer transfer);
    }
}

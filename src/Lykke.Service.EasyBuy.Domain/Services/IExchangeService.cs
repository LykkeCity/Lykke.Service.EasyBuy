using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IExchangeService
    {
        Task<string> TransferAsync(string sourceWalletId, string destinationWalletId, string assetId, decimal amount,
            string transactionId = null);
    }
}

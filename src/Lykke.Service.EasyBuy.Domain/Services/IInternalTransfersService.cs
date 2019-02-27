using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IInternalTransfersService
    {
        Task TransferAsync(
            string transferId,
            string easyBuyClientId,
            string userWalletId,
            bool direct,
            string assetId,
            decimal amount);
    }
}

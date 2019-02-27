using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IPricesService
    {
        Task<PriceSnapshot> CreateSnapshotAsync(string walletId, string assetPair, OrderType type, decimal quotingVolume);
        Task<PriceSnapshot> GetSnapshotAsync(string id);
    }
}

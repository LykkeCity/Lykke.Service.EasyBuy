using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IPricesGenerator
    {
        Task StartAll();

        Task StopAll();

        Task Start(string assetPair);

        Task Stop(string assetPair);
    }
}

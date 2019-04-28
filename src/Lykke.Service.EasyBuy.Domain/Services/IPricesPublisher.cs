using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IPricesPublisher
    {
        Task PublishAsync(Price price);
    }
}

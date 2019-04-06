using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.EasyBuy.Domain.Services
{
    public interface IPricesGenerator
    {
        Task TryExecuteAsync(CancellationToken cancellationToken);
    }
}

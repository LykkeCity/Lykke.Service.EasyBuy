using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Refit;

namespace Lykke.Service.EasyBuy.Client.Api
{
    /// <summary>
    /// Provides methods to work with exchanges.
    /// </summary>
    [PublicAPI]
    public interface IExchangesApi
    {
        /// <summary>
        /// Returns a collection of available exchanges.
        /// </summary>
        /// <returns>A list of exchanges.</returns>
        [Get("/api/exchanges")]
        Task<IReadOnlyList<string>> GetAsync();
    }
}

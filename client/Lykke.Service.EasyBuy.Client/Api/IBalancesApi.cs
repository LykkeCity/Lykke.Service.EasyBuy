using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;
using Refit;

namespace Lykke.Service.EasyBuy.Client.Api
{
    /// <summary>
    /// Provides methods to request balance.
    /// </summary>
    [PublicAPI]
    public interface IBalancesApi
    {
        /// <summary>
        /// Returns EasyBuy wallet balances.
        /// </summary>
        /// <returns>A list of balances per asset.</returns>
        [Get("/api/balances")]
        Task<IReadOnlyList<BalanceModel>> GetAsync();
    }
}

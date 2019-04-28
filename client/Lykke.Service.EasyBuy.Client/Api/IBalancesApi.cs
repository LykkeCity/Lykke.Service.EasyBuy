using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.EasyBuy.Client.Models;
using Lykke.Service.EasyBuy.Client.Models.Balances;
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
        /// <exception cref="ClientApiException">If an unexpected error occurred while getting balances.</exception>
        [Get("/api/balances")]
        Task<IReadOnlyList<BalanceModel>> GetAsync();
    }
}

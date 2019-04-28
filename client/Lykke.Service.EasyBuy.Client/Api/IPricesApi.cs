using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.EasyBuy.Client.Models.Prices;
using Refit;

namespace Lykke.Service.EasyBuy.Client.Api
{
    /// <summary>
    /// Provides method to work with prices.
    /// </summary>
    [PublicAPI]
    public interface IPricesApi
    {
        /// <summary>
        /// Returns a price by identifier.
        /// </summary>
        /// <param name="priceId">The identifier of price.</param>
        /// <returns>The price.</returns>
        /// <exception cref="ClientApiException">If price does not exist.</exception>
        [Get("/api/prices/{priceId}")]
        Task<PriceModel> GetByIdAsync(string priceId);

        /// <summary>
        /// Returns a collection of active prices.
        /// </summary>
        /// <returns>A collection of prices.</returns>
        [Get("/api/prices")]
        Task<IReadOnlyList<PriceModel>> GetAllAsync();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;
using Refit;

namespace Lykke.Service.EasyBuy.Client.Api
{
    /// <summary>
    /// Provides methods to work with instruments.
    /// </summary>
    [PublicAPI]
    public interface IInstrumentsApi
    {
        /// <summary>
        /// Returns a collection of instruments.
        /// </summary>
        /// <returns>A collection of instruments.</returns>
        [Get("/api/instruments")]
        Task<IReadOnlyCollection<InstrumentModel>> GetAllAsync();

        /// <summary>
        /// Returns an instrument by asset pair id.
        /// </summary>
        /// <param name="assetPairId">The asses pair id.</param>
        /// <returns>An instrument.</returns>
        [Get("/api/instruments/{assetPairId}")]
        Task<InstrumentModel> GetByAssetPairIdAsync(string assetPairId);

        /// <summary>
        /// Adds new instrument settings (without levels).
        /// </summary>
        /// <param name="model">The model that describes instrument.</param>
        [Post("/api/instruments")]
        Task AddAsync([Body] InstrumentEditModel model);

        /// <summary>
        /// Updates instrument settings (without levels).
        /// </summary>
        /// <param name="model">The model that describes instrument.</param>
        [Put("/api/instruments")]
        Task UpdateAsync([Body] InstrumentEditModel model);

        /// <summary>
        /// Deletes the instrument settings by asset pair id.
        /// </summary>
        /// <param name="assetPairId">The asses pair id.</param>
        [Delete("/api/instruments/{assetPairId}")]
        Task DeleteAsync(string assetPairId);
    }
}

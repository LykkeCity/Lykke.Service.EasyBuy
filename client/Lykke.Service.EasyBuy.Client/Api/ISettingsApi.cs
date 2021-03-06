using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models;
using Refit;

namespace Lykke.Service.EasyBuy.Client.Api
{
    /// <summary>
    /// Provides methods to work with EasyBuy settings.
    /// </summary>
    [PublicAPI]
    public interface ISettingsApi
    {
        /// <summary>
        /// Returns account settings.
        /// </summary>
        /// <returns>Account settings.</returns>
        [Get("/api/settings/account")]
        Task<AccountSettingsModel> GetAccountSettingsAsync();

        /// <summary>
        /// Returns default price settings.
        /// </summary>
        /// <returns>Default price settings.</returns>
        [Get("/api/settings/default")]
        Task<DefaultSettingsModel> GetDefaultSettingsAsync();

        /// <summary>
        /// Used to update default settings.
        /// </summary>
        /// <param name="defaultSettings">New default settings.</param>
        [Put("/api/settings/default")]
        Task UpdateDefaultPriceSettingsAsync(DefaultSettingsModel defaultSettings);
    }
}

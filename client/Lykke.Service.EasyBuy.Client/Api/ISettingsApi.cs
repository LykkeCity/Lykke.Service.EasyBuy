using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.EasyBuy.Client.Models.Settings;
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
        [Get("/api/Settings/account")]
        Task<AccountSettingsModel> GetAccountSettingsAsync();
    }
}

using System.Net;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models.Settings;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class SettingsController : Controller, ISettingsApi
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        /// <inheritdoc/>
        [HttpGet("account")]
        [ProducesResponseType(typeof(AccountSettingsModel), (int) HttpStatusCode.OK)]
        public Task<AccountSettingsModel> GetAccountSettingsAsync()
        {
            return Task.FromResult(new AccountSettingsModel
            {
                WalletId = _settingsService.GetWalletId()
            });
        }
    }
}

using System.Net;
using System.Threading.Tasks;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models;
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
        public async Task<AccountSettingsModel> GetAccountSettingsAsync()
        {
            return new AccountSettingsModel
            {
                ClientId = await _settingsService.GetClientIdAsync()
            };
        }

        /// <inheritdoc/>
        [HttpGet("prices")]
        [ProducesResponseType(typeof(PriceSettingsModel), (int) HttpStatusCode.OK)]
        public async Task<PriceSettingsModel> GetDefaultPriceSettingsAsync()
        {
            return new PriceSettingsModel
            {
                Markup = await _settingsService.GetDefaultMarkupAsync(),
                Lifetime = await _settingsService.GetDefaultPriceLifetimeAsync()
            };
        }
    }
}

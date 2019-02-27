using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class BalancesController : Controller, IBalancesApi
    {
        public readonly IBalancesService _balancesService;
        
        public BalancesController(
            IBalancesService balancesService)
        {
            _balancesService = balancesService;
        }
        
        /// <inheritdoc/>
        /// <response code="200">A collection of instruments.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<BalanceModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<BalanceModel>> GetAsync()
        {
            return Mapper.Map<IReadOnlyList<BalanceModel>>(await _balancesService.GetAsync());
        }
    }
}

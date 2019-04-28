using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models.Balances;
using Lykke.Service.EasyBuy.Domain.Entities.Balances;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class BalancesController : Controller, IBalancesApi
    {
        private readonly IBalancesService _balancesService;

        public BalancesController(IBalancesService balancesService)
        {
            _balancesService = balancesService;
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of instruments.</response>
        [HttpGet]
        [ResponseCache(Duration = 5)]
        [ProducesResponseType(typeof(IReadOnlyCollection<BalanceModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<BalanceModel>> GetAsync()
        {
            try
            {
                IReadOnlyList<Balance> balances = await _balancesService.GetAllAsync();

                return Mapper.Map<List<BalanceModel>>(balances);
            }
            catch (FailedOperationException exception)
            {
                throw new ValidationApiException(HttpStatusCode.BadRequest, exception.Message);
            }
        }
    }
}

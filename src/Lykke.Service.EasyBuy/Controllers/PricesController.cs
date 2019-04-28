using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models.Prices;
using Lykke.Service.EasyBuy.Domain.Entities.Prices;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class PricesController : Controller, IPricesApi
    {
        private readonly IPriceService _priceService;
        private readonly ILog _log;
        
        public PricesController(
            IPriceService priceService,
            ILogFactory logFactory)
        {
            _priceService = priceService;
            _log = logFactory.CreateLog(this);
        }

        /// <inheritdoc/>
        [HttpGet("{priceId}")]
        [ProducesResponseType(typeof(PriceModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<PriceModel> GetByIdAsync(string priceId)
        {
            try
            {
                var price = await _priceService.GetByIdAsync(priceId);

                return Mapper.Map<PriceModel>(price);
            }
            catch (EntityNotFoundException)
            {
                throw new ValidationApiException(HttpStatusCode.NotFound, "Price does not exist.");
            }
        }

        /// <inheritdoc/>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PriceModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<PriceModel>> GetAllAsync()
        {
            IReadOnlyList<Price> prices = await _priceService.GetActiveAsync();
            
            return Mapper.Map<IReadOnlyList<PriceModel>>(prices);
        }
    }
}

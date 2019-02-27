using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.Log;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class PricesController : Controller, IPricesApi
    {
        private readonly IPricesService _pricesService;
        private readonly ILog _log;
        
        public PricesController(
            IPricesService pricesService,
            ILogFactory logFactory)
        {
            _pricesService = pricesService;
            _log = logFactory.CreateLog(this);
        }

        /// <inheritdoc/>
        [HttpGet("{priceId}")]
        [ProducesResponseType(typeof(PriceModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<PriceModel> GetPriceAsync(string priceId)
        {
            try
            {
                var price = await _pricesService.GetSnapshotAsync(priceId);

                return Mapper.Map<PriceModel>(price);
            }
            catch (EntityNotFoundException)
            {
                throw new ValidationApiException(HttpStatusCode.NotFound, "Price snapshot does not exist.");
            }
        }

        /// <inheritdoc/>
        [ProducesResponseType(typeof(PriceModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<PriceModel> CalculatePriceAsync([FromBody] CalculatePriceModel model)
        {
            try
            {
                var price = await _pricesService.CreateSnapshotAsync(
                    model.WalletId,
                    model.AssetPair,
                    model.Type == OrderType.Buy ? Domain.OrderType.Buy : Domain.OrderType.Sell,
                    model.QuotingVolume);

                return Mapper.Map<PriceModel>(price);
            }
            catch (OperationFailedException exception)
            {
                _log.Error(exception);
                
                throw new ValidationApiException(HttpStatusCode.BadRequest, exception.Message);
            }
        }
    }
}

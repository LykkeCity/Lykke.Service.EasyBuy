using System;
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
using Lykke.Service.EasyBuy.Domain;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class InstrumentsController : Controller, IInstrumentsApi
    {
        private readonly IInstrumentsService _instrumentsService;

        public InstrumentsController(
            IInstrumentsService instrumentsService)
        {
            _instrumentsService = instrumentsService;
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of instruments.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<InstrumentModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<InstrumentModel>> GetAllAsync()
        {
            var instruments = await _instrumentsService.GetAllAsync();

            return Mapper.Map<List<InstrumentModel>>(instruments);
        }

        /// <inheritdoc/>
        /// <response code="200">An instrument.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpGet("{assetPairId}")]
        [ProducesResponseType(typeof(InstrumentModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<InstrumentModel> GetByAssetPairIdAsync(string assetPairId)
        {
            try
            {
                var instrument = await _instrumentsService.GetByAssetPairIdAsync(assetPairId);

                return Mapper.Map<InstrumentModel>(instrument);
            }
            catch (EntityNotFoundException)
            {
                throw new ValidationApiException(HttpStatusCode.NotFound, "Instrument does not exist.");
            }
        }
        
        /// <inheritdoc/>
        /// <response code="204">The instrument successfully added.</response>
        /// <response code="400">The instrument already used.</response>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AddAsync([FromBody] InstrumentEditModel model)
        {
            try
            {
                var instrument = Mapper.Map<Instrument>(model);

                await _instrumentsService.AddAsync(instrument);
            }
            catch (InvalidOperationException exception)
            {
                throw new ValidationApiException(HttpStatusCode.BadRequest, exception.Message);
            }
        }

        /// <inheritdoc/>
        /// <response code="204">The instrument successfully updated.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task UpdateAsync([FromBody] InstrumentEditModel model)
        {
            try
            {
                var instrument = Mapper.Map<Instrument>(model);

                await _instrumentsService.UpdateAsync(instrument);
            }
            catch (EntityNotFoundException)
            {
                throw new ValidationApiException(HttpStatusCode.NotFound, "Instrument does not exist.");
            }
        }

        /// <inheritdoc/>
        /// <response code="204">The instrument successfully deleted.</response>
        /// <response code="400">An error occurred while deleting instrument.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpDelete("{assetPairId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task DeleteAsync(string assetPairId)
        {
            try
            {
                await _instrumentsService.DeleteAsync(assetPairId);
            }
            catch (EntityNotFoundException)
            {
                throw new ValidationApiException(HttpStatusCode.NotFound, "Instrument does not exist.");
            }
            catch (InvalidOperationException exception)
            {
                throw new ValidationApiException(HttpStatusCode.BadRequest, exception.Message);
            }
        }
    }
}

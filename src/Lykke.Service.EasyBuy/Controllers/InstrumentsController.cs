using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models.Instruments;
using Lykke.Service.EasyBuy.Domain.Entities.Instruments;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class InstrumentsController : Controller, IInstrumentsApi
    {
        private readonly IInstrumentService _instrumentService;

        public InstrumentsController(IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of instruments.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<InstrumentModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<InstrumentModel>> GetAllAsync()
        {
            IReadOnlyList<Instrument> instruments = await _instrumentService.GetAllAsync();

            return Mapper.Map<List<InstrumentModel>>(instruments);
        }

        /// <inheritdoc/>
        /// <response code="200">An instrument.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpGet("{assetPairId}")]
        [ProducesResponseType(typeof(InstrumentModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<InstrumentModel> GetByIdAsync(string instrumentId)
        {
            Instrument instrument = await _instrumentService.GetByIdAsync(instrumentId);

            if (instrument == null)
                throw new ValidationApiException(HttpStatusCode.NotFound, "Instrument does not exist.");

            return Mapper.Map<InstrumentModel>(instrument);
        }

        /// <inheritdoc/>
        /// <response code="204">The instrument successfully added.</response>
        /// <response code="400">The instrument already used.</response>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AddAsync([FromBody] InstrumentModel model)
        {
            try
            {
                var instrument = Mapper.Map<Instrument>(model);

                await _instrumentService.AddAsync(instrument);
            }
            catch (EntityAlreadyExistsException)
            {
                throw new ValidationApiException(HttpStatusCode.BadRequest, "The instrument already exists");
            }
        }

        /// <inheritdoc/>
        /// <response code="204">The instrument successfully updated.</response>
        /// <response code="400">Instrument could not be updated.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task UpdateAsync([FromBody] InstrumentModel model)
        {
            try
            {
                var instrumentSettings = Mapper.Map<Instrument>(model);

                await _instrumentService.UpdateAsync(instrumentSettings);
            }
            catch (EntityNotFoundException)
            {
                throw new ValidationApiException(HttpStatusCode.NotFound, "Instrument does not exist.");
            }
            catch (FailedOperationException exception)
            {
                throw new ValidationApiException(HttpStatusCode.BadRequest, exception.Message);
            }
        }

        /// <inheritdoc/>
        /// <response code="204">The instrument successfully deleted.</response>
        /// <response code="400">An error occurred while deleting instrument.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpDelete("{instrumentId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task DeleteAsync(string instrumentId)
        {
            try
            {
                await _instrumentService.DeleteAsync(instrumentId);
            }
            catch (EntityNotFoundException)
            {
                throw new ValidationApiException(HttpStatusCode.NotFound, "Instrument does not exist.");
            }
            catch (FailedOperationException exception)
            {
                throw new ValidationApiException(HttpStatusCode.BadRequest, exception.Message);
            }
        }
    }
}

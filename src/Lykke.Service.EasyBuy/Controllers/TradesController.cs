using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models;
using Lykke.Service.EasyBuy.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class TradesController : Controller, ITradesApi
    {
        private readonly ITradesRepository _tradesRepository;
        
        public TradesController(
            ITradesRepository tradesRepository)
        {
            _tradesRepository = tradesRepository;
        }
        
        /// <inheritdoc />
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<TradeModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<TradeModel>> GetTradesAsync(DateTime fromDate, DateTime toDate)
        {
            return Mapper.Map<IReadOnlyList<TradeModel>>(await _tradesRepository.GetAsync(fromDate, toDate));
        }
    }
}

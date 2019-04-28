using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.EasyBuy.Client.Api;
using Lykke.Service.EasyBuy.Client.Models.Orders;
using Lykke.Service.EasyBuy.Domain.Entities.Orders;
using Lykke.Service.EasyBuy.Domain.Exceptions;
using Lykke.Service.EasyBuy.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.EasyBuy.Controllers
{
    [Route("/api/[controller]")]
    public class OrdersController : Controller, IOrdersApi
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <inheritdoc />
        /// <response code="404">Order does not exist.</response>
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<OrderModel> GetByIdAsync(string orderId)
        {
            Order order = await _orderService.GetByIdAsync(orderId);

            if (order == null)
                throw new ValidationApiException(HttpStatusCode.NotFound, "Order does not exist.");

            return Mapper.Map<OrderModel>(order);
        }

        /// <inheritdoc />
        /// <response code="400">Request parameters not valid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<OrderModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyList<OrderModel>> GetAsync(string clientId, string assetPair, DateTime? dateFrom,
            DateTime? dateTo, int skip, int take)
        {
            if (skip < 0)
                throw new ValidationApiException(HttpStatusCode.BadRequest, "Skip should be positive integer.");

            if (take < 0)
                throw new ValidationApiException(HttpStatusCode.BadRequest, "Take should be positive integer.");

            IReadOnlyList<Order> orders =
                await _orderService.GetAllAsync(clientId, assetPair, dateFrom, dateTo, skip, take);

            return Mapper.Map<IReadOnlyList<OrderModel>>(orders);
        }

        /// <inheritdoc />
        /// <response code="400">An error occurred while creating order.</response>
        [HttpPost]
        [ProducesResponseType(typeof(OrderModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<OrderModel> CreateAsync([FromBody] CreateOrderModel model)
        {
            try
            {
                var order = await _orderService.CreateAsync(model.ClientId, model.PriceId, model.QuotingVolume);

                return Mapper.Map<OrderModel>(order);
            }
            catch (FailedOperationException exception)
            {
                throw new ValidationApiException(HttpStatusCode.BadRequest, exception.Message);
            }
        }
    }
}

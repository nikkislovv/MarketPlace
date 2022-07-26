﻿using ApiApplication.Contracts;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.OrderDTO;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IProductManager _productManager;
        private readonly UserManager<User> _userManager;



        public OrdersController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IProductManager productManager,UserManager<User> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _productManager = productManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Get list of Orders
        /// </summary>
        /// <param name="orderParameters"></param>
        /// <returns>The orders list</returns>
        /// <response code="200">Returns the list of all Orders</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">No Orders in the database</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllOrdersAsync([FromQuery] OrderParameters orderParameters)//получить может админ;клиент и селлер(если они его создавали)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirst(e => e.Type == "Id").Value);
            var role =await _userManager.GetRolesAsync(user);
            if (orderParameters.SearchTerm != User.FindFirst(e => e.Type == "Id").Value && !role.Contains("admin"))
            {
                return Forbid();
            }
            var orders = await _repository.Order.GetAllOrdersAsync(orderParameters,true);
            if (orders.Count() == 0)
            {
                _logger.LogInfo($"No Orders in the database.");
                return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(orders.MetaData));
            var ordersDto = _mapper.Map<IEnumerable<OrderToShowDto>>(orders);
            return Ok(ordersDto);
        }


        /// <summary>
        /// Get one Order by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>One Order</returns>
        /// <response code="200">Returns one Order</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Order with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{Id}")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] Guid Id)//получить может админ;клиент и селлер(если они его создавали)
        {
            var order = HttpContext.Items["order"] as Order;
            var user = await _userManager.FindByIdAsync(User.FindFirst(e => e.Type == "Id").Value);
            var role = await _userManager.GetRolesAsync(user);
            if (order.UserId != User.FindFirst(e => e.Type == "Id").Value && !role.Contains("admin"))
            {
                return Forbid();
            }
            var orderDto= _mapper.Map<OrderToShowDto>(order);
            return Ok(orderDto);
        }

        /// <summary>
        /// Create one new Order
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        /// <response code="201">Order created</response>
        /// <response code="400">New Order is null</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Product in the order is out of warehouse(Or there is no order in the db)</response>
        /// <response code="422">New Order not valid</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Authorize(Roles = "client,seller")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrderAsync([FromBody]OrderToCreateDto orderDto)
        {
            if (!await _productManager.CheckForAvailability(orderDto.ProductsIds))//проверяем на наличие продуктов
            {
                _logger.LogInfo($"Product in the order is out of warehouse(Or there is no order in the db)");
                return NotFound();
            }
            var order = _mapper.Map<Order>(orderDto);
            order.UserId= User.FindFirst(e => e.Type == "Id").Value;
            if (!await _productManager.AddProductCollection(order, orderDto.ProductsIds))
            {
                _logger.LogWarn($"{nameof(CreateOrderAsync)}: AddModifiedPhoneCollection failed");
                return StatusCode(500, "Internal server error");
            }
            _repository.Order.CreateOrder(order);
            await _repository.SaveAsync();
            if (!await _productManager.ControlOfQuantity(order.Products))//устанавливаем другое количество продукта 
            {
                _logger.LogWarn($"{nameof(CreateOrderAsync)}: ControlOfQuantity failed");
                return StatusCode(500, "Internal server error");
            }
            return StatusCode(201);
        }

        /// <summary>
        /// Update Order by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        /// <response code="204">Order updated</response>
        /// <response code="400">New Order is null</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Order with this id not found</response>
        /// <response code="422">New Order not valid</response>
        /// <response code="500">Server error</response>
        [HttpPut("{Id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateOrderAsync([FromRoute] Guid Id,OrderToUpdateDto orderDto)
        {
            var order = HttpContext.Items["order"] as Order;
            var user = await _userManager.FindByIdAsync(User.FindFirst(e => e.Type == "Id").Value);
            var role =await _userManager.GetRolesAsync(user);
            if (order.UserId != User.FindFirst(e => e.Type == "Id").Value && !role.Contains("admin"))
            {
                return Forbid();
            }
            _mapper.Map(orderDto, order);
            //_productManager.ClearProductsInCollection(order);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete Order by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <response code="204">Order deleted</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Order with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete("{Id}")]
        [ServiceFilter(typeof(ValidateOrderExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteOrderAsync([FromRoute] Guid Id)//удалить может админ;клиент и селлер(если они его создавали)
        {
            var order = HttpContext.Items["order"] as Order;
            var user = await _userManager.FindByIdAsync(User.FindFirst(e => e.Type == "Id").Value);
            var role = await _userManager.GetRolesAsync(user);
            if (order.UserId != User.FindFirst(e => e.Type == "Id").Value && !role.Contains("admin"))
            {
                return Forbid();
            }
            _repository.Order.DeleteOrder(order);
            await _repository.SaveAsync();
            return NoContent();
        }
        
    }
}

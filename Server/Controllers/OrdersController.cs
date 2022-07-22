using ApiApplication.Contracts;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.OrderDTO;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.ActionFilters;
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


        public OrdersController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IProductManager productManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _productManager = productManager;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllOrdersAsync([FromQuery] OrderParameters orderParameters)
        {
            var orders = await _repository.Order.GetAllOrdersAsync(orderParameters,true);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(orders.MetaData));
            if (orders.Count() == 0)
            {
                _logger.LogInfo($"No Products in the database.");
                return NotFound();
            }
            var ordersDto = _mapper.Map<IEnumerable<OrderToShowDto>>(orders);
            return Ok(ordersDto);
        }



        [HttpPost]
        [Authorize(Roles = "client,seller")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

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

        
    }
}

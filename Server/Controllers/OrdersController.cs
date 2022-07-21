using ApiApplication.Contracts;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.OrderDTO;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Authorize(Roles = "client,seller")]
        public async Task<IActionResult> CreateOrderAsync([FromBody]OrderToCreateDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.UserId= User.FindFirst(e => e.Type == "Id").Value;
            if (!await _productManager.AddProductCollection(order, orderDto))
            {
                _logger.LogWarn($"{nameof(CreateOrderAsync)}: AddModifiedPhoneCollection failed");
                return StatusCode(500, "Internal server error");
            }
            _repository.Order.CreateOrder(order);
            await _repository.SaveAsync();
            return StatusCode(201);
        }
    }
}

using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.ProductDTO;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ProductsController(IRepositoryManager repository, ILoggerManager logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsAsync()//Получение всех продуктов
        { 
            var products = await _repository.Product.GetAllProductsAsync(true);
            if (products.Count()==0)
            {
                _logger.LogInfo($"No Products in the database.");
                return NotFound();
            }
            var productsDto=_mapper.Map<IEquatable<ProductToShowDto>>(products);
            return Ok(productsDto);
        }
        [AllowAnonymous]
        [HttpGet("{productId}", Name = "GetProductByIdAsync")]
        public async Task<IActionResult> GetProductByIdAsync(Guid productId)//Получение продукта по Id
        {
            var product = await _repository.Product.GetProductByIdAsync(productId, true);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }
            var productDto=_mapper.Map<ProductToShowDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> CreateProductForWarehouseAsync([FromQuery]Guid warehouseId, [FromBody] ProductToCreateDto productDto)//Создание продукта(только seller)
        {
            var warehouse = await _repository.Warehouse.GetWarehouseByIdAsync(warehouseId,false);
            if (warehouse == null)
            {
                _logger.LogInfo($"Warehouse with id: {warehouseId} doesn't exist in the database.");
                return NotFound();
            }
            var product = _mapper.Map<Product>(productDto);
            product.UserId = User.FindFirst(e => e.Type == "Id").Value;
            _repository.Product.CreateProductForWarehouse(warehouseId, product);
            await _repository.SaveAsync();
            return StatusCode(201);
        }

    }
}

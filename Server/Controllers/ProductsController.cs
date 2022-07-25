using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.ProductDTO;
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
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;



        public ProductsController(IRepositoryManager repository, ILoggerManager logger,IMapper mapper, UserManager<User> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Get list of all Products
        /// </summary>
        /// <param name="productParameters"></param>
        /// <returns>The products list</returns>
        /// <response code="200">Returns the list of all Products</response>
        /// <response code="400">Request parameters are not valid</response>
        /// <response code="404">No Products in the database</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] ProductParameters productParameters)//Получение всех продуктов
        {

            if (!productParameters.ValidPriceRange)
                return BadRequest("MaxPrice can notbe less then MinPrice.");
            var products = await _repository.Product.GetAllProductsAsync(productParameters,true);
            if (products.Count()==0)
            {
                _logger.LogInfo($"No Products in the database.");
                return NotFound();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));
            var productsDto=_mapper.Map<IEnumerable<ProductToShowDto>>(products);
            return Ok(productsDto);
        }

        /// <summary>
        /// Get Product by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>One Product</returns>
        /// <response code="200">Returns one Product</response>
        /// <response code="404">Product with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{Id}")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetProductByIdAsync([FromRoute]Guid Id)//Получение продукта по Id
        {
            var product = HttpContext.Items["product"] as Product;
            var productDto =_mapper.Map<ProductToShowDto>(product);
            return Ok(productDto);
        }

        /// <summary>
        /// Create one new Product
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        /// <response code="201">Product created</response>
        /// <response code="400">New Product is null</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Warehouse with this id not found</response>
        /// <response code="422">New Product not valid</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Authorize(Roles = "seller")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProducAsync([FromBody] ProductToCreateDto productDto)//Создание продукта с привязкой к скалду через dto(только seller)
        {
            var warehouse = await _repository.Warehouse.GetWarehouseByIdAsync(productDto.WarehouseId,false);//не можем использовать атрибут ,тк в атрибуте id берем из контекста запроса
            if (warehouse == null)
            {
                _logger.LogInfo($"Warehouse with id: {productDto.WarehouseId} doesn't exist in the database.");
                return NotFound();
            }
            var product = _mapper.Map<Product>(productDto);
            product.UserId = User.FindFirst(e => e.Type == "Id").Value;
            _repository.Product.CreateProduct(product);
            await _repository.SaveAsync();
            return StatusCode(201);
        }

        /// <summary>
        /// Update Product by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        /// <response code="204">Product updated</response>
        /// <response code="400">New Product is null</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Product with this id not found</response>
        /// <response code="422">New Product not valid</response>
        /// <response code="500">Server error</response>
        [HttpPut("{Id}")]
        [Authorize(Roles = "admin,seller")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid Id,[FromBody] ProductToUpdateDto productDto)//обновление продукта, (admin) или seller (который создавал)
        {
            var product = HttpContext.Items["product"] as Product;
            var user = await _userManager.FindByIdAsync(User.FindFirst(e => e.Type == "Id").Value);
            var role = await _userManager.GetRolesAsync(user);
            if (product.UserId != User.FindFirst(e => e.Type == "Id").Value && !role.Contains("admin"))
            {
                return Forbid();
            }
            _mapper.Map(productDto,product);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete Product by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <response code="204">Product deleted</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Product with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin,seller")]
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProductAsync([FromRoute]Guid Id)//удаление продукта, (admin) или seller (который создавал)
        {
            var product = HttpContext.Items["product"] as Product;
            var user = await _userManager.FindByIdAsync(User.FindFirst(e => e.Type == "Id").Value);
            var role = await _userManager.GetRolesAsync(user);
            if (product.UserId != User.FindFirst(e => e.Type == "Id").Value && !role.Contains("admin"))
            {
                return Forbid();
            }
            _repository.Product.DeleteProduct(product);
            await _repository.SaveAsync();
            return NoContent();
        }

    }
}

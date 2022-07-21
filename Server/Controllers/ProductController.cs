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


        public ProductsController(IRepositoryManager repository, ILoggerManager logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] ProductParameters productParameters)//Получение всех продуктов
        {
            if (!productParameters.ValidPriceRange)
                return BadRequest("MaxPrice can notbe less then MinPrice.");
            var products = await _repository.Product.GetAllProductsAsync(productParameters,true);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));
            if (products.Count()==0)
            {
                _logger.LogInfo($"No Products in the database.");
                return NotFound();
            }
            var productsDto=_mapper.Map<IEnumerable<ProductToShowDto>>(products);
            return Ok(productsDto);
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute]Guid Id)//Получение продукта по Id
        {
            var product = await _repository.Product.GetProductByIdAsync(Id, true);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {Id} doesn't exist in the database.");
                return NotFound();
            }
            var productDto=_mapper.Map<ProductToShowDto>(product);
            return Ok(productDto);
        }


        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> CreateProducAsync([FromBody] ProductToCreateDto productDto)//Создание продукта с привязкой к скалду через dto(только seller)
        {
            var warehouse = await _repository.Warehouse.GetWarehouseByIdAsync(productDto.WarehouseId,false);
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


        [HttpPut("{Id}")]
        [Authorize(Roles = "admin,seller")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid Id,[FromBody] ProductToUpdateDto productDto)//обновление продукта, (admin) или seller (который создавал)
        {
            var product =await  _repository.Product.GetProductByIdAsync(Id, true);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {Id} doesn't exist in the database.");
                return NotFound();
            }
            if (product.UserId != User.FindFirst(e => e.Type == "Id").Value && "admin" != User.FindFirst(e => e.Type == "Id").Value)
            {
                return Forbid();
            }
            var productToUpdate=_mapper.Map(productDto,product);
            await _repository.SaveAsync();
            return NoContent();
        }


        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin,seller")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute]Guid Id)//удаление продукта, (admin) или seller (который создавал)
        {
            var product=await _repository.Product.GetProductByIdAsync(Id,false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {Id} doesn't exist in the database.");
                return NotFound();
            }
            if (product.UserId != User.FindFirst(e => e.Type == "Id").Value &&"admin"!= User.FindFirst(e => e.Type == "Id").Value)
            {
                return Forbid();
            }
            _repository.Product.DeleteProduct(product);
            await _repository.SaveAsync();
            return NoContent();
        }

    }
}

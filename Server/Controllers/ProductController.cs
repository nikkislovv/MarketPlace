using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.ProductDTO;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public ProductsController(IRepositoryManager repository, ILoggerManager logger,IMapper mapper,UserManager<User> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
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

        //[HttpGet("ShowBySeller/{sellerId}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetProductsBySeller([FromRoute]Guid sellerId)//получение всех продуктов продовца
        //{
        //    var seller = await _userManager.FindByIdAsync(sellerId.ToString());
        //    var userRole=await _userManager.GetRolesAsync(seller);
        //    if (seller == null || !userRole.Contains("seller"))
        //    {
        //        _logger.LogInfo($"Seller with id: {sellerId} doesn't exist in the database.");
        //        return NotFound();
        //    }
        //    var products = await _repository.Product.GetProductsByAccountAsync(sellerId.ToString(), true);
        //    if (products.Count() == 0)
        //    {
        //        _logger.LogInfo($"No Products in the database.");
        //        return NotFound();
        //    }
        //    var productsDto = _mapper.Map<IEnumerable<ProductToShowDto>>(products);
        //    return Ok(productsDto);
        //}


        //[HttpGet("ShowByWarehose/{warehouseId}")]
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> GetProductsByWarehouseAsync([FromRoute]Guid warehouseId)
        //{
        //    var warehouse = await _repository.Warehouse.GetWarehouseByIdAsync(warehouseId,false);
        //    if (warehouse == null)
        //    {
        //        _logger.LogInfo($"Warehouse with id: {warehouseId} doesn't exist in the database.");
        //        return NotFound();
        //    }
        //    var products=await _repository.Product.GetProductsByWarehouseAsync(warehouseId,true);
        //    if (products.Count() == 0)
        //    {
        //        _logger.LogInfo($"No Products in the database.");
        //        return NotFound();
        //    }
        //    var productsDto = _mapper.Map<IEnumerable<ProductToShowDto>>(products);
        //    return Ok(productsDto); 
        //}


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

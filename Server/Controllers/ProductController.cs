using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet("ShowAll")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = await _repository.Product.GetAllProductsAsync(false);
                var productsDto=_mapper.Map<IEquatable<ProductToShowDto>>(products);
                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllProductsAsync)}action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

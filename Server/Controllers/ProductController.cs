using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.ProductDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet("ShowAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsAsync()
        { 
            var products = await _repository.Product.GetAllProductsAsync(true);
            var productsDto=_mapper.Map<IEquatable<ProductToShowDto>>(products);
            return Ok(productsDto);
        }


    }
}

using ApiApplication.Contracts;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.FeedbackDTO;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.ActionFilters;
using System;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/Products/{productId}/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IProductManager _productManager;



        public FeedbacksController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper,UserManager<User> userManager,IProductManager productManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _productManager = productManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "client,seller")]
        public async Task<IActionResult> CreateFeedbackAsync(Guid productId, [FromBody] FeedbackToCreateDto feedbackDto)
        {

            var product = await _repository.Product.GetProductByIdAsync(productId, true);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }
            var userId = User.FindFirst(e => e.Type == "Id").Value;
            if (!_productManager.CheckProductInOrders(product.Orders,userId))
            {
                return Forbid();
            }
            var feedback = _mapper.Map<Feedback>(feedbackDto);
            feedback.UserId = userId;
            _repository.Feedback.CreateFeedbackForProduct(productId, feedback);
            await _repository.SaveAsync();
            return StatusCode(201);
        }
    }
}

using ApiApplication.Contracts;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.FeedbackDTO;
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


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeedbacksByProductAsync(Guid productId, [FromQuery] FeedbackParameters feedbackParameters)
        {
            var product = await _repository.Product.GetProductByIdAsync(productId, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }
            var feedbacks = await _repository.Feedback.GetFeedbacksByProductAsync(productId, feedbackParameters, true);
            if (feedbacks.Count() == 0)
            {
                _logger.LogInfo($"No Feedbacks for this product in the database.");
                return NotFound();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(feedbacks.MetaData));
            var feedbacksDto = _mapper.Map<IEnumerable<FeedbackToShowDto>>(feedbacks);
            return Ok(feedbacksDto);
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
            if (!_productManager.CheckProductInOrders(product.Orders,userId) || _productManager.CheckFeedbackInProduct(product.Feedbacks,userId))//разрешает только если заказывал продукт(запрещает если уже оставлял отзыв для продукта)
            {
                return Forbid();
            }
            var feedback = _mapper.Map<Feedback>(feedbackDto);
            feedback.UserId = userId;
            _repository.Feedback.CreateFeedbackForProduct(productId, feedback);
            await _repository.SaveAsync();
            return StatusCode(201);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteOrderAsync([FromRoute] Guid Id)//удалить может админ;клиент и селлер(если они его создавали)
        {
            var order = await _repository.Order.GetOrderByIdAsync(Id, false);
            if (order == null)
            {
                _logger.LogInfo($"Order with id: {Id} doesn't exist in the database.");
                return NotFound();
            }
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

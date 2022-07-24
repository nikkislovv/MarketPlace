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
    [Authorize]
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
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        public async Task<IActionResult> GetFeedbacksByProductAsync([FromRoute]Guid productId, [FromQuery] FeedbackParameters feedbackParameters)
        {
            var product = HttpContext.Items["product"] as Product;
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
        [Authorize(Roles = "client,seller")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        public async Task<IActionResult> CreateFeedbackAsync([FromRoute]Guid productId, [FromBody] FeedbackToCreateDto feedbackDto)
        {
            var product = HttpContext.Items["product"] as Product;
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
        [ServiceFilter(typeof(ValidateProductExistsAttribute))]
        public async Task<IActionResult> DeletefeedbackForProductAsync([FromRoute] Guid productId, [FromRoute] Guid Id)//удалить может админ;клиент и селлер(если они его создавали)
        {
            var product = HttpContext.Items["product"] as Product;
            var feedback =await _repository.Feedback.GetFeedbackByIdByProductAsync(productId,Id,true);
            if (feedback == null)
            {
                _logger.LogInfo($"Feedback with id: {Id} doesn't exist in the database.");
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(User.FindFirst(e => e.Type == "Id").Value);
            var role = await _userManager.GetRolesAsync(user);
            if (feedback.UserId != User.FindFirst(e => e.Type == "Id").Value && !role.Contains("admin"))
            {
                return Forbid();
            }
            _repository.Feedback.DeleteFeedback(feedback);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}

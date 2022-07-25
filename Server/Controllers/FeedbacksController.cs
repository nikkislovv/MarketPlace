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

        /// <summary>
        /// Get Feedbacks for Product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="feedbackParameters"></param>
        /// <returns>The products list</returns>
        /// <response code="200">Returns the list of all DeliveryPoints</response>
        /// <response code="404">No Product in the database</response>
        /// <response code="404">No Feedbacks for this product in the database</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFeedbacksByProductAsync([FromRoute]Guid productId, [FromQuery] FeedbackParameters feedbackParameters)
        {
            var product=await _repository.Product.GetProductByIdAsync(productId,false);
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

        /// <summary>
        /// Create Feedback for Product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="feedbackDto"></param>
        /// <returns></returns>
        /// <response code="201">Feedback created</response>
        /// <response code="400">New Feedback is null</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="404">No Product in the database</response>
        /// <response code="403">You have no rules</response>
        /// <response code="422">New Feedback not valid</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Authorize(Roles = "client,seller")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateFeedbackAsync([FromRoute]Guid productId, [FromBody] FeedbackToCreateDto feedbackDto)
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

        /// <summary>
        /// Delete Feedback for Product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <response code="204">Feedback deleted</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Product with this id not found</response>
        /// <response code="404">Feedback with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete("{Id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletefeedbackForProductAsync([FromRoute] Guid productId, [FromRoute] Guid Id)//удалить может админ;клиент и селлер(если они его создавали)
        {
            var product = await _repository.Product.GetProductByIdAsync(productId, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }
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

using ApiApplication.Contracts;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.DelveryPointDTO;
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
    public class DeliveryPointsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IProductManager _productManager;

        public DeliveryPointsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IProductManager productManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _productManager = productManager;
        }


        /// <summary>
        /// Get list of all DeliveryPoints
        /// </summary>
        /// <param name="deliveryPointParameters"></param>
        /// <returns>The DeliveryPoints list</returns>
        /// <response code="200">Returns the list of all DeliveryPoints</response>
        /// <response code="404">No DeliveryPoints in the database</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllDeliveryPointsAsync([FromQuery] DeliveryPointParameters deliveryPointParameters)
        {
            var deliveryPoints = await _repository.DeliveryPoint.GetAllDeliveryPointsAsync(deliveryPointParameters, false);
            if (deliveryPoints.Count() == 0)
            {
                _logger.LogInfo($"No DeliveryPoints in the database.");
                return NotFound();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(deliveryPoints.MetaData));
            var deliveryPointsDto = _mapper.Map<IEnumerable<DeliveryPointToShowDto>>(deliveryPoints);
            return Ok(deliveryPointsDto);
        }

        /// <summary>
        /// Get one DeliveryPoint by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>One DeliveryPoint</returns>
        /// <response code="200">Returns one DeliveryPoint</response>
        /// <response code="404">DeliveryPoint with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{Id}")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidateDeliveryPointExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetDeliveryPointByIdAsync([FromRoute] Guid Id)
        {
            var deliveryPoint = HttpContext.Items["deliveryPoint"] as DeliveryPoint;
            var productDto = _mapper.Map<DeliveryPointToShowDto>(deliveryPoint);
            return Ok(productDto);
        }

        /// <summary>
        /// Create one new DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointDto"></param>
        /// <returns></returns>
        /// <response code="201">DeliveryPoint created</response>
        /// <response code="400">New DeliveryPoint is null</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="422">New DeliveryPoint not valid</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProducAsync([FromBody] DeliveryPointToCreateDto deliveryPointDto)
        {
            var deliveryPoint = _mapper.Map<DeliveryPoint>(deliveryPointDto);
            _repository.DeliveryPoint.CreateDeliveryPoint(deliveryPoint);
            await _repository.SaveAsync();
            return StatusCode(201);
        }

        /// <summary>
        /// Update DeliveryPoint by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="deliveryPointDto"></param>
        /// <returns></returns>
        /// <response code="204">DeliveryPoint updated</response>
        /// <response code="400">New DeliveryPoint is null</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">DeliveryPoint with this id not found</response>
        /// <response code="422">New DeliveryPoint not valid</response>
        /// <response code="500">Server error</response>
        [HttpPut("{Id}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateDeliveryPointExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateDeliveryPointAsync([FromRoute] Guid Id, [FromBody] DeliveryPointToUpdateDto deliveryPointDto)
        {
            var deliveryPoint = HttpContext.Items["deliveryPoint"] as DeliveryPoint;
            _mapper.Map(deliveryPointDto, deliveryPoint);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete DeliveryPoint by id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <response code="204">DeliveryPoint deleted</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">DeliveryPoint with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidateDeliveryPointExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteDeliveryPointAsync([FromRoute] Guid Id)
        {
            var deliveryPoint = HttpContext.Items["deliveryPoint"] as DeliveryPoint;
            _repository.DeliveryPoint.DeleteDeliveryPoint(deliveryPoint);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}

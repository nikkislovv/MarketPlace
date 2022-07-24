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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDeliveryPointsAsync([FromQuery] DeliveryPointParameters deliveryPointParameters)
        {
            var deliveryPoints = await _repository.DeliveryPoint.GetAllDeliveryPointsAsync(deliveryPointParameters, false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(deliveryPoints.MetaData));
            if (deliveryPoints.Count() == 0)
            {
                _logger.LogInfo($"No DeliveryPoints in the database.");
                return NotFound();
            }
            var deliveryPointsDto = _mapper.Map<IEnumerable<DeliveryPointToShowDto>>(deliveryPoints);
            return Ok(deliveryPointsDto);
        }


        [HttpGet("{Id}")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidateDeliveryPointExistsAttribute))]
        public IActionResult GetDeliveryPointByIdAsync([FromRoute] Guid Id)
        {
            var deliveryPoint = HttpContext.Items["deliveryPoint"] as DeliveryPoint;
            var productDto = _mapper.Map<DeliveryPointToShowDto>(deliveryPoint);
            return Ok(productDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateProducAsync([FromBody] DeliveryPointToCreateDto deliveryPointDto)
        {
            var deliveryPoint = _mapper.Map<DeliveryPoint>(deliveryPointDto);
            _repository.DeliveryPoint.CreateDeliveryPoint(deliveryPoint);
            await _repository.SaveAsync();
            return StatusCode(201);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateDeliveryPointExistsAttribute))]
        public async Task<IActionResult> UpdateDeliveryPointAsync([FromRoute] Guid Id, [FromBody] DeliveryPointToUpdateDto deliveryPointDto)
        {
            var deliveryPoint = HttpContext.Items["deliveryPoint"] as DeliveryPoint;
            _mapper.Map(deliveryPointDto, deliveryPoint);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidateDeliveryPointExistsAttribute))]
        public async Task<IActionResult> DeleteDeliveryPointAsync([FromRoute] Guid Id)
        {
            var deliveryPoint = HttpContext.Items["deliveryPoint"] as DeliveryPoint;
            _repository.DeliveryPoint.DeleteDeliveryPoint(deliveryPoint);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}

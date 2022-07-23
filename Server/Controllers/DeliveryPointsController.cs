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
        public async Task<IActionResult> GetAllDeliveryPointsAsync([FromQuery] DeliveryPointParameters deliveryPointParameters)//Получение всех точек доставок
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



    }
}

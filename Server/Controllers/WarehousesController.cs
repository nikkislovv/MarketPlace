using ApiApplication.Contracts;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.WarehouseDTO;
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
    public class WarehousesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IProductManager _productManager;

        public WarehousesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IProductManager productManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _productManager = productManager;
        }

        /// <summary>
        /// Get list of all Warehouses
        /// </summary>
        /// <param name="warehouseParameters"></param>
        /// <returns>The Warehouses list</returns>
        /// <response code="200">Returns the list of all Warehouses</response>
        /// <response code="404">No Warehouses in the database</response>
        /// <response code="500">Server error</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllWarehousesAsync([FromQuery] WarehouseParameters warehouseParameters)
        {
            var warehouses = await _repository.Warehouse.GetAllWarehousesAsync(warehouseParameters, false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(warehouses.MetaData));
            if (warehouses.Count() == 0)
            {
                _logger.LogInfo($"No Warehouses in the database.");
                return NotFound();
            }
            var warehousesDto = _mapper.Map<IEnumerable<WarehouseToShowDto>>(warehouses);
            return Ok(warehousesDto);
        }

        /// <summary>
        /// Get Warehouse by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>One Warehouse</returns>
        /// <response code="200">Returns one Warehouse</response>
        /// <response code="404">Warehouse with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpGet("{Id}")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidateWarehouseExistsAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetlWarehouseByIdAsync([FromRoute] Guid Id)
        {
            var warehouse = HttpContext.Items["warehouse"] as Warehouse;
            var warehouseDto = _mapper.Map<WarehouseToShowDto>(warehouse);
            return Ok(warehouseDto);
        }

        /// <summary>
        /// Create one new Warehouse
        /// </summary>
        /// <param name="warehouseDto"></param>
        /// <returns></returns>
        /// <response code="201">Warehouse created</response>
        /// <response code="400">New Warehouse is null</response>
        /// <response code="401">You are not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="422">New Warehouse not valid</response>
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
        public async Task<IActionResult> CreateWarehouseAsync([FromBody] WarehouseToCreateDto warehouseDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseDto);
            _repository.Warehouse.CreateWarehouse(warehouse);
            await _repository.SaveAsync();
            return StatusCode(201);
        }

        /// <summary>
        /// Update Warehouse by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="warehouseDto"></param>
        /// <returns></returns>
        /// <response code="204">Warehouse updated</response>
        /// <response code="400">New Warehouse is null</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Warehouse with this id not found</response>
        /// <response code="422">New Warehouse not valid</response>
        /// <response code="500">Server error</response>
        [HttpPut("{Id}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateWarehouseExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateWarehouseAsync([FromRoute] Guid Id, [FromBody] WarehouseToUpdateDto warehouseDto)
        {
            var warehouse = HttpContext.Items["warehouse"] as Warehouse;          
            _mapper.Map(warehouseDto, warehouse);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete Warehouse by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <response code="204">Warehouse deleted</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">You have no rules</response>
        /// <response code="404">Warehouse with this id not found</response>
        /// <response code="500">Server error</response>
        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidateWarehouseExistsAttribute))]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteWarehouseAsync([FromRoute] Guid Id)
        {
            var warehouse = HttpContext.Items["warehouse"] as Warehouse;
            _repository.Warehouse.DeleteWarehouse(warehouse);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}

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


        [HttpGet]
        [AllowAnonymous]
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


        [HttpGet("{Id}")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidateWarehouseExistsAttribute))]
        public IActionResult GetlWarehouseByIdAsync([FromRoute] Guid Id)
        {
            var warehouse = HttpContext.Items["warehouse"] as Warehouse;
            var warehouseDto = _mapper.Map<WarehouseToShowDto>(warehouse);
            return Ok(warehouseDto);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateWarehouseAsync([FromBody] WarehouseToCreateDto warehouseDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseDto);
            _repository.Warehouse.CreateWarehouse(warehouse);
            await _repository.SaveAsync();
            return StatusCode(201);
        }


        [HttpPut("{Id}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateWarehouseExistsAttribute))]
        public async Task<IActionResult> UpdateDeliveryPointAsync([FromRoute] Guid Id, [FromBody] WarehouseToUpdateDto warehouseDto)
        {
            var warehouse = HttpContext.Items["warehouse"] as Warehouse;          
            _mapper.Map(warehouseDto, warehouse);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ValidateWarehouseExistsAttribute))]
        public async Task<IActionResult> DeleteWarehouseAsync([FromRoute] Guid Id)
        {
            var warehouse = HttpContext.Items["warehouse"] as Warehouse;
            _repository.Warehouse.DeleteWarehouse(warehouse);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}

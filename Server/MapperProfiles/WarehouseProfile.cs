using AutoMapper;
using Entities.DataTransferObjects.WarehouseDTO;
using Entities.Models;

namespace Server.MapperProfiles
{
    public class WarehouseProfile:Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseToShowDto>();
            CreateMap<WarehouseToCreateDto, Warehouse>();
            CreateMap<WarehouseToUpdateDto, Warehouse>();

        }
    }
}

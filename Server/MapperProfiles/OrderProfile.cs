using AutoMapper;
using Entities.DataTransferObjects.OrderDTO;
using Entities.Models;

namespace Server.MapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderToCreateDto, Order>();

        }
    }
}

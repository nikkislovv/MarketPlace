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
            CreateMap<OrderToUpdateDto, Order>();
            CreateMap<Order, OrderToShowDto>()
                    .ForMember(c => c.DeliveryPointAddress, opt => opt.MapFrom(x => x.DeliveryPoint.Address))
                    .ForMember(c => c.UserName, opt => opt.MapFrom(x => x.User.FullName));




        }
    }
}

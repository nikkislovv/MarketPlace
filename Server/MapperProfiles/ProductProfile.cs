using AutoMapper;
using Entities.DataTransferObjects.ProductDTO;
using Entities.Models;

namespace Server.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductToShowDto>()
                    .ForMember(c => c.WarehouseAddress, opt => opt.MapFrom(x => x.Warehouse.Address))
                    .ForMember(c => c.SellerName, opt => opt.MapFrom(x => x.User.FullName))
                    .ForMember(c => c.SellerName, opt => opt.MapFrom(x => x.User.FullName));
            CreateMap<ProductToShowDto, Product>();
            CreateMap<ProductToCreateDto, Product>();


        }
    }
}

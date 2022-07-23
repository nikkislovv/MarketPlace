using AutoMapper;
using Entities.DataTransferObjects.DelveryPointDTO;
using Entities.DataTransferObjects.FeedbackDTO;
using Entities.Models;

namespace Server.MapperProfiles
{
    public class DeliveryPointProfile:Profile
    {
        public DeliveryPointProfile()
        {
            CreateMap<DeliveryPoint, DeliveryPointToShowDto>();
            //CreateMap<Feedback, FeedbackToShowDto>()
            //        .ForMember(c => c.ProductName, opt => opt.MapFrom(x => x.Product.Name))
            //        .ForMember(c => c.UserName, opt => opt.MapFrom(x => x.User.FullName));


        }
    }
}

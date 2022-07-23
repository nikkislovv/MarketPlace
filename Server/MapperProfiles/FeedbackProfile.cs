using AutoMapper;
using Entities.DataTransferObjects.FeedbackDTO;
using Entities.Models;

namespace Server.MapperProfiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<FeedbackToCreateDto, Feedback>();
            CreateMap<Feedback, FeedbackToShowDto>()
                    .ForMember(c => c.ProductName, opt => opt.MapFrom(x => x.Product.Name))
                    .ForMember(c => c.UserName, opt => opt.MapFrom(x => x.User.FullName));


        }
    }
}

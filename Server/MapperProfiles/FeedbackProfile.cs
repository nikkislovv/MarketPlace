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
            

        }
    }
}

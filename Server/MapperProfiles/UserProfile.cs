using AutoMapper;
using Entities.DataTransferObjects.AccountDTO;
using Entities.Models;

namespace Server.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}

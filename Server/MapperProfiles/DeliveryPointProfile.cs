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
            CreateMap<DeliveryPointToCreateDto, DeliveryPoint>();
            CreateMap<DeliveryPointToUpdateDto, DeliveryPoint>();



        }
    }
}

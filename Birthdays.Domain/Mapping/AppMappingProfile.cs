using Data.Entities;
using Domain.DTO;
using Domain.DTO.Requests.Auth;
using Domain.Models;

namespace Domain.Mapping;

public class AppMappingProfile : MappingProfileBase
{
    public AppMappingProfile()
    {
        CreateMap<Profile, ProfileDto>();
        CreateMap<Subscription, SubscriptionDto>();

        CreateMap<LoginRequest, LoginModel>();
    }
}
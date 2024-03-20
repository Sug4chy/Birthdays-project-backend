using Data.Entities;
using Domain.DTO;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Profiles;
using Domain.Models;

namespace Domain.Mapping;

public class AppMappingProfile : MappingProfileBase
{
    public AppMappingProfile()
    {
        CreateMap<Profile, ProfileDto>();
        CreateMap<WishList, WishListDto>();
        CreateMap<Wish, WishDto>();

        CreateMap<DateOnly, DateDto>();
        CreateMap<User, MainPageProfileDto>();
        
        CreateMap<LoginRequest, LoginModel>();
    }
}
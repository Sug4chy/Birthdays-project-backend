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
        CreateMap<WishList, WishListDto>();
        CreateMap<Wish, WishDto>();
        
        CreateMap<LoginRequest, LoginModel>();
    }
}
using AutoMapper;
using Domain.Accessors;
using Domain.DTO;
using Domain.DTO.Responses.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.WishLists;

namespace Domain.Handlers.WishLists;

public class GetCurrentProfileWishListsHandler(
    ICurrentUserAccessor userAccessor,
    IProfileService profileService,
    IWishListService wishListService,
    IMapper mapper)
{
    public async Task<GetProfileWishListsResponse> Handle(CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        if (!await profileService.CheckIfProfileExistsAsync(currentUser.ProfileId, ct))
        {
            throw new NotFoundException
            {
                Error = ProfilesErrors.NoSuchProfileWithId(currentUser.ProfileId)
            };
        }

        var wishLists = await wishListService
            .GetWishListsByProfileIdAsync(currentUser.ProfileId, ct);
        return new GetProfileWishListsResponse
        {
            WishLists = wishLists.Select(mapper.Map<WishListDto>).ToArray()
        };
    }
}
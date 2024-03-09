using AutoMapper;
using Domain.Accessors;
using Domain.DTO;
using Domain.DTO.Responses.WishLists;
using Domain.Services.WishLists;

namespace Domain.Handlers.WishLists;

public class GetCurrentProfileWishListsHandler(
    ICurrentUserAccessor userAccessor,
    IWishListService wishListService,
    IMapper mapper)
{
    public async Task<GetProfileWishListsResponse> Handle(CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        var wishLists = await wishListService
            .GetWishListsByProfileIdAsync(currentUser.ProfileId, ct);
        return new GetProfileWishListsResponse
        {
            WishLists = wishLists.Select(mapper.Map<WishListDto>).ToArray()
        };
    }
}
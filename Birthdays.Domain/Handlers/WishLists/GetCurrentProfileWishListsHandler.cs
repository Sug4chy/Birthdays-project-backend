using AutoMapper;
using Domain.Accessors;
using Domain.DTO;
using Domain.DTO.Responses.WishLists;
using Domain.Services.WishLists;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class GetCurrentProfileWishListsHandler(
    ICurrentUserAccessor userAccessor,
    IWishListService wishListService,
    IMapper mapper,
    ILogger<GetCurrentProfileWishListsHandler> logger)
{
    public async Task<GetProfileWishListsResponse> Handle(CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        logger.LogInformation($"GetCurrentProfileWishListsRequest was received " +
                              $"from user with email {currentUser.Email}.");
        
        var wishLists = await wishListService
            .GetWishListsByProfileIdAsync(currentUser.ProfileId, ct);
        
        logger.LogInformation($"WishLists of user with email {currentUser.Email} " +
                              $"were successfully sent to him.");
        return new GetProfileWishListsResponse
        {
            WishLists = wishLists.Select(mapper.Map<WishListDto>).ToArray()
        };
    }
}
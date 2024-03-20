using Domain.Accessors;
using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Profiles;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class CreateWishListHandler(
    ICurrentUserAccessor userAccessor,
    CreateWishListRequestValidator validator,
    IProfileService profileService,
    IWishListService wishListService,
    ILogger<CreateWishListHandler> logger)
{
    public async Task Handle(CreateWishListRequest request,
        CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        
        logger.LogInformation($"CreateWishList request was received from {currentUser.Email} user");
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var profile = await profileService.GetProfileByIdAsync(currentUser.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, ProfilesErrors.NoSuchProfileWithId(currentUser.ProfileId));

        await wishListService.CreateWishListAsync(request.WishList, profile!, ct);
        logger.LogInformation($"CreateWishList response was successfully sent to {currentUser.Email} user");
    }
}
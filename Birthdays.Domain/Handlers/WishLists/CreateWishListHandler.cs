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
        
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with {currentUser.Email}.");
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var profile = await profileService.GetProfileByIdAsync(currentUser.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, ProfilesErrors.NoSuchProfileWithId(currentUser.ProfileId));

        var newWishListId = await wishListService.CreateWishListAsync(request.WishList, profile!, ct);
        logger.LogInformation($"User with email {currentUser.Email} successfully " +
                              $"created wish list with id {newWishListId}.");
    }
}
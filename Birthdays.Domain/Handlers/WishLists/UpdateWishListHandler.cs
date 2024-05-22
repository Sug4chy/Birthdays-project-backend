using Domain.Accessors;
using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class UpdateWishListHandler(
    ICurrentUserAccessor userAccessor,
    UpdateWishListRequestValidator validator,
    IWishListService wishListService,
    ILogger<UpdateWishListHandler> logger)
{
    public async Task Handle(UpdateWishListRequest request, CancellationToken ct = default)
    {
        string currentUserEmail = await userAccessor.GetCurrentUserEmailAsync(ct);
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with email {currentUserEmail}.");
        
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId!.Value, ct);
        NotFoundException.ThrowIfNull(wishList, 
            WishListsErrors.NoSuchWishListWithId(request.WishListId.Value));
        await wishListService.UpdateWishListAsync(wishList!, request.NewName, request.NewDescription, ct);
        logger.LogInformation($"User with email {currentUserEmail} successfully updated " +
                              $"wish list with id {wishList!.Id}.");
    }
}
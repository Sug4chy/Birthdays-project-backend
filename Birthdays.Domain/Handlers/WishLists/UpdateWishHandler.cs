using Domain.Accessors;
using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class UpdateWishHandler(
    ICurrentUserAccessor userAccessor,
    UpdateWishRequestValidator validator,
    IWishListService wishListService,
    ILogger<UpdateWishHandler> logger)
{
    public async Task Handle(UpdateWishRequest request, CancellationToken ct = default)
    {
        string currentUserEmail = await userAccessor.GetCurrentUserEmailAsync(ct);
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with email {currentUserEmail}.");
        
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId!.Value, ct);
        NotFoundException.ThrowIfNull(wishList, 
            WishListsErrors.NoSuchWishListWithId(request.WishListId!.Value));

        var wish = wishList!.Wishes!.FirstOrDefault(w => w.Id == request.WishId!.Value);
        NotFoundException.ThrowIfNull(wish, 
            WishListsErrors.NoSuchWishWithId(request.WishId!.Value));

        await wishListService.UpdateWishAsync(wish!, request.Wish, ct);
        logger.LogInformation($"User with email {currentUserEmail} successfully updated " +
                              $"wish with id {wish!.Id}.");
    }
}
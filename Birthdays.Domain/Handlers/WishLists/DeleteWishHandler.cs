using Domain.Accessors;
using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class DeleteWishHandler(
    ICurrentUserAccessor userAccessor,
    DeleteWishRequestValidator validator,
    IWishListService wishListService,
    ILogger<DeleteWishHandler> logger)
{
    public async Task Handle(DeleteWishRequest request, CancellationToken ct = default)
    {
        string currentUserEmail = await userAccessor.GetCurrentUserEmailAsync(ct);
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with email {currentUserEmail}.");
        
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId, ct);
        NotFoundException.ThrowIfNull(wishList, 
            WishListsErrors.NoSuchWishListWithId(request.WishListId));

        var wish = wishList!.Wishes!.FirstOrDefault(w => w.Id == request.WishId);
        NotFoundException.ThrowIfNull(wish, WishListsErrors.NoSuchWishWithId(request.WishId));

        await wishListService.DeleteWishAsync(wish!, ct);
        logger.LogInformation($"User with email {currentUserEmail} successfully " +
                              $"deleted wish with id {wish!.Id}.");
    }
}
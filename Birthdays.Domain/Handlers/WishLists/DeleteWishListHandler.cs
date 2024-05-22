using Domain.Accessors;
using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class DeleteWishListHandler(
    ICurrentUserAccessor userAccessor,
    DeleteWishListRequestValidator validator,
    IWishListService wishListService,
    ILogger<DeleteWishListHandler> logger)
{
    public async Task Handle(DeleteWishListRequest request, CancellationToken ct = default)
    {
        string currentUserEmail = await userAccessor.GetCurrentUserEmailAsync(ct);
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with email {currentUserEmail}.");
        
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId, ct);
        NotFoundException.ThrowIfNull(wishList, 
            WishListsErrors.NoSuchWishListWithId(request.WishListId));

        await wishListService.DeleteWishListAsync(wishList!, ct);
        logger.LogInformation($"User with email {currentUserEmail} successfully " +
                              $"deleted wish list with id {wishList!.Id}.");
    }
}
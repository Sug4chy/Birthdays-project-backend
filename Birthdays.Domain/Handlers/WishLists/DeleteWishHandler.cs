using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;

namespace Domain.Handlers.WishLists;

public class DeleteWishHandler(
    DeleteWishRequestValidator validator,
    IWishListService wishListService)
{
    public async Task Handle(DeleteWishRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId, ct);
        NotFoundException.ThrowIfNull(wishList, 
            WishListsErrors.NoSuchWishListWithId(request.WishListId));

        var wish = wishList!.Wishes!.FirstOrDefault(w => w.Id == request.WishId);
        NotFoundException.ThrowIfNull(wish, WishListsErrors.NoSuchWishWithId(request.WishId));

        await wishListService.DeleteWishAsync(wish!, ct);
    }
}
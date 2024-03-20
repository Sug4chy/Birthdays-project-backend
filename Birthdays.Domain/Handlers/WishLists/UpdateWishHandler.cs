using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;

namespace Domain.Handlers.WishLists;

public class UpdateWishHandler(
    UpdateWishRequestValidator validator,
    IWishListService wishListService)
{
    public async Task Handle(UpdateWishRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId!.Value, ct);
        NotFoundException.ThrowIfNull(wishList, 
            WishListsErrors.NoSuchWishListWithId(request.WishListId!.Value));

        var wish = wishList!.Wishes!.FirstOrDefault(w => w.Id == request.WishId!.Value);
        NotFoundException.ThrowIfNull(wish, 
            WishListsErrors.NoSuchWishWithId(request.WishId!.Value));

        await wishListService.UpdateWishAsync(wish!, request.Wish, ct);
    }
}
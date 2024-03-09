using Domain.DTO.Requests.WishLists;
using Domain.DTO.Responses.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;

namespace Domain.Handlers.WishLists;

public class UpdateWishListHandler(
    UpdateWishListRequestValidator validator,
    IWishListService wishListService)
{
    public async Task<UpdateWishListResponse> Handle(UpdateWishListRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId!.Value, ct);
        NotFoundException.ThrowIfNull(wishList, 
            WishListsErrors.NoSuchWishListWithId(request.WishListId.Value));
        await wishListService.UpdateWishListAsync(wishList!, request.NewName, request.NewDescription, ct);

        return new UpdateWishListResponse();
    }
}
using Domain.DTO.Requests.WishLists;
using Domain.DTO.Responses.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;

namespace Domain.Handlers.WishLists;

public class DeleteWishListHandler(
    DeleteWishListRequestValidator validator,
    IWishListService wishListService)
{
    public async Task<DeleteWishListResponse> Handle(DeleteWishListRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId, ct);
        NotFoundException.ThrowIfNull(wishList, 
            WishListsErrors.NoSuchWishListWithId(request.WishListId));

        await wishListService.DeleteWishListAsync(wishList!, ct);
        return new DeleteWishListResponse();
    }
}
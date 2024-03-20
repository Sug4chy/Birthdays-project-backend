using Domain.Accessors;
using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;

namespace Domain.Handlers.WishLists;

public class CreateWishHandler(
    ICurrentUserAccessor userAccessor,
    CreateWishRequestValidator validator,
    IWishListService wishListService)
{
    public async Task Handle(CreateWishRequest request, CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);

        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var wishList = await wishListService.GetWishListByIdAsync(request.WishListId!.Value, ct);
        NotFoundException.ThrowIfNull(wishList, WishListsErrors.NoSuchWishListWithId(request.WishListId.Value));

        if (currentUser.ProfileId != wishList!.BirthdayManId)
        {
            throw new ForbiddenException
            {
                Error = WishListsErrors
                    .WishListDoesntBelongToUser(wishList.Id, Guid.Parse(currentUser.Id))
            };
        }

        await wishListService.CreateWishAsync(request.Wish, wishList, ct);
    }
}
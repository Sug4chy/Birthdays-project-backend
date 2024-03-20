using Domain.Accessors;
using Domain.DTO.Requests.WishLists;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.WishLists;
using Domain.Validators.WishLists;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.WishLists;

public class CreateWishHandler(
    ICurrentUserAccessor userAccessor,
    CreateWishRequestValidator validator,
    IWishListService wishListService,
    ILogger<CreateWishHandler> logger)
{
    public async Task Handle(CreateWishRequest request, CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with email {currentUser.Email}.");

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

        var newWishId = await wishListService.CreateWishAsync(request.Wish, wishList, ct);
        logger.LogInformation($"User with email {currentUser.Email} successfully " +
                              $"created wish with id {newWishId}.");
    }
}
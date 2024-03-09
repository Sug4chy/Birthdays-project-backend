namespace Domain.Results;

public static class WishListsErrors
{
    public static Error NoSuchWishListWithId(Guid id)
        => new(nameof(NoSuchWishListWithId), $"WishList with id {id} doesn't exist");

    public static Error WishListDoesntBelongToUser(Guid wlId, Guid userId)
        => new(nameof(WishListDoesntBelongToUser), 
            $"WishList with id {wlId} doesn't belong to user with id {userId}");
}
namespace Domain.DTO.Requests.WishLists;

public record UpdateWishRequest
{
    public Guid? WishListId { get; init; }
    public Guid? WishId { get; init; }
    public required WishDto Wish { get; init; }
}
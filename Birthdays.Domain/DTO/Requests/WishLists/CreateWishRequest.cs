namespace Domain.DTO.Requests.WishLists;

public record CreateWishRequest
{
    public Guid? WishListId { get; init; }
    public required WishDto Wish { get; init; }
}
namespace Domain.DTO.Requests.WishLists;

public record DeleteWishRequest
{
    public required Guid WishListId { get; init; }
    public required Guid WishId { get; init; }
}
namespace Domain.DTO.Requests.WishLists;

public record DeleteWishListRequest
{
    public required Guid WishListId { get; init; }
}
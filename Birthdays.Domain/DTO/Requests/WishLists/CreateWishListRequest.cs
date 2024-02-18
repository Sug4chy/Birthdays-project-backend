namespace Domain.DTO.Requests.WishLists;

public record CreateWishListRequest
{
    public required WishListDto WishList { get; init; }
}
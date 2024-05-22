namespace Domain.DTO.Responses.WishLists;

public record GetProfileWishListsResponse
{
    public required WishListDto[] WishLists { get; init; }
}
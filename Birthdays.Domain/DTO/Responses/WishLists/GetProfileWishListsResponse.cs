namespace Domain.DTO.Responses.WishLists;

public record GetProfileWishListsResponse : IResponse
{
    public required WishListDto[] WishLists { get; init; }
}
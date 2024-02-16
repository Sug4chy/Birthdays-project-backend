namespace Domain.DTO.Requests.WishLists;

public record CreateWishListRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
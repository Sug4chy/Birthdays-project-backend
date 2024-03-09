namespace Domain.DTO.Requests.WishLists;

public record UpdateWishListRequest
{
    public Guid? WishListId { get; init; }
    public required string NewName { get; init; }
    public string? NewDescription { get; init; }
}
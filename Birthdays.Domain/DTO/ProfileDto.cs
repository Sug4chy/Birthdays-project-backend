namespace Domain.DTO;

public record ProfileDto
{
    public required Guid Id { get; init; }
    public string? Description { get; init; }
    public required WishListDto[] WishLists { get; init; }
}
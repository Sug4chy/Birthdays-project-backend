namespace Domain.DTO;

public record WishListDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required WishDto[] Wishes { get; init; } = Array.Empty<WishDto>();
}
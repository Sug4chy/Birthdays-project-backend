namespace Domain.DTO;

public record WishDto
{
    public required string Name { get; init; }
    public string? GiftRef { get; init; }
    public string? Description { get; init; }
}
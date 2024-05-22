namespace Domain.DTO;

public record WishDto
{
    public Guid? Id { get; init; }
    public required string Name { get; init; }
    public string? GiftRef { get; init; }
    public string? Description { get; init; }
}
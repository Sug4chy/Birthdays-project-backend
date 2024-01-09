namespace Domain.DTO;

public record SubscriptionDto
{
    public required Guid BirthdayManId { get; init; }
    public required Guid SubscriberId { get; init; }
}
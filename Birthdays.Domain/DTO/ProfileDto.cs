namespace Domain.DTO;

public record ProfileDto
{
    public string? Description { get; init; }
    public SubscriptionDto[] SubscriptionsAsBirthdayMan { get; init; } 
        = Array.Empty<SubscriptionDto>();
    public SubscriptionDto[] SubscriptionsAsSubscriber { get; init; }
        = Array.Empty<SubscriptionDto>();
}
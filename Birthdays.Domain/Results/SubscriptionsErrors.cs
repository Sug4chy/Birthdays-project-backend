namespace Domain.Results;

public static class SubscriptionsErrors
{
    public static Error NoSuchSubscription(Guid subscriberId, Guid birthdayManId)
        => new(nameof(NoSuchSubscription),
            $"User with id {subscriberId} isn't subscribed to user with id {birthdayManId}");
}
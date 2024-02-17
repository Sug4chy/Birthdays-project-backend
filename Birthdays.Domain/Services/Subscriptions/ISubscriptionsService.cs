namespace Domain.Services.Subscriptions;

public interface ISubscriptionsService
{
    Task<bool> IsSubscribedToAsync(Guid subscriberId, Guid birthdayManId, CancellationToken ct = default);
    Task SubscribeAsync(Guid subscriberId, Guid birthdayManId, CancellationToken ct = default);
}
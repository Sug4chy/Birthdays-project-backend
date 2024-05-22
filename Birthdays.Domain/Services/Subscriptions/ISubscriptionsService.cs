using Data.Entities;
using Domain.Results;

namespace Domain.Services.Subscriptions;

public interface ISubscriptionsService
{
    Task<bool> IsSubscribedToAsync(Guid subscriberId, Guid birthdayManId, CancellationToken ct = default);
    Task SubscribeAsync(Guid subscriberId, Guid birthdayManId, CancellationToken ct = default);
    Task<Result> UnsubscribeAsync(Guid subscriberId, Guid birthdayManId, CancellationToken ct = default);

    Task<Subscription[]> GetSubscriptionsByProfileIdWithPaginationAsync(Guid subscriberId, int pageIndex,
        CancellationToken ct = default);
}
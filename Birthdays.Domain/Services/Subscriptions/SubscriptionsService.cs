using Data.Context;
using Data.Entities;
using Domain.Results;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Subscriptions;

public class SubscriptionsService(AppDbContext context) : ISubscriptionsService
{
    public Task<bool> IsSubscribedToAsync(Guid subscriberId, Guid birthdayManId,
        CancellationToken ct = default)
        => context.Subscriptions
            .AnyAsync(s => s.BirthdayManId == birthdayManId
                           && s.SubscriberId == subscriberId, ct);

    public async Task SubscribeAsync(Guid subscriberId, Guid birthdayManId, CancellationToken ct = default)
    {
        await context.Subscriptions.AddAsync(new Subscription
        {
            BirthdayManId = birthdayManId,
            SubscriberId = subscriberId
        }, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<Result> UnsubscribeAsync(Guid subscriberId, Guid birthdayManId, CancellationToken ct = default)
    {
        var subscription = await context.Subscriptions
            .FirstOrDefaultAsync(s => s.BirthdayManId == birthdayManId
                                      && s.SubscriberId == subscriberId, ct);
        if (subscription is null)
        {
            return Result.Failure(SubscriptionsErrors.NoSuchSubscription(subscriberId, birthdayManId));
        }

        context.Subscriptions.Remove(subscription);
        await context.SaveChangesAsync(ct);
        return Result.Success();
    }

    public Task<Subscription[]> GetSubscriptionsByProfileIdWithPaginationAsync(Guid subscriberId, int pageIndex,
        CancellationToken ct = default)
        => context.Subscriptions
            .Include(s => s.BirthdayMan)
            .ThenInclude(p => p!.User)
            .Where(s => s.SubscriberId == subscriberId)
            .Skip(10 * pageIndex)
            .Take(10)
            .ToArrayAsync(ct);
}
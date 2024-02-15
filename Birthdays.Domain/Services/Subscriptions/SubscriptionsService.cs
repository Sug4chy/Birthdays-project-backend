using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Subscriptions;

public class SubscriptionsService(AppDbContext context) : ISubscriptionsService
{
    public Task<bool> IsSubscribedToAsync(Guid subscriberId, Guid birthdayManId,
        CancellationToken ct = default)
        => context.Subscriptions
            .AnyAsync(s => s.BirthdayManId == birthdayManId
                           && s.SubscriberId == subscriberId, ct);
}
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Subscriptions;

public class SubscriptionsService(AppDbContext context) : ISubscriptionsService
{
    public async Task<bool> IsSubscribedToAsync(Guid subscriberId, Guid birthdayManId,
        CancellationToken ct = default)
    {
        var subscription = await context.Subscriptions
            .FirstOrDefaultAsync(s => s.BirthdayManId == birthdayManId 
                                      && s.SubscriberId == subscriberId, ct);
        return subscription is not null;
    }
}
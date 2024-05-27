using Data.Context;
using Data.Entities;
using Domain.Accessors;
using Microsoft.EntityFrameworkCore;

namespace Domain.Handlers.Profiles;

public class SubscribeAllHandler(
    ICurrentUserAccessor userAccessor,
    AppDbContext dbContext
)
{
    public async Task Handle(CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);

        var profiles = await GetAllUnsubscribedProfilesExceptAsync(currentUser.ProfileId, ct);
        foreach (var profile in profiles)
        {
            await CreateSubscriptionAsync(currentUser.ProfileId, profile.Id, ct);
        }

        await dbContext.SaveChangesAsync(ct);
    }

    private Task<List<Profile>> GetAllUnsubscribedProfilesExceptAsync(Guid profileId, CancellationToken ct = default)
        => dbContext.Profiles
            .Include(p => p.SubscriptionsAsBirthdayMan)
            .Where(
                p => p.Id != profileId
                     && p.SubscriptionsAsBirthdayMan
                         .All(s => s.SubscriberId != profileId)
            )
            .ToListAsync(ct);

    private async Task CreateSubscriptionAsync(Guid subscriberId, Guid birthdayManId,
        CancellationToken ct = default)
    {
        await dbContext.Subscriptions
            .AddAsync(new Subscription
            {
                BirthdayManId = birthdayManId,
                SubscriberId = subscriberId
            }, ct);
    }
}
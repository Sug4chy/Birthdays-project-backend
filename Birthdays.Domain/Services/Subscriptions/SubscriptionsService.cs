﻿using Data.Context;
using Data.Entities;
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
}
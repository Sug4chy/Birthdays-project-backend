using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Profiles;

public class ProfileService(AppDbContext context) : IProfileService
{
    public async Task<Profile> CreateAsync(CancellationToken ct = default)
    {
        var newProfile = new Profile();
        await context.Profiles.AddAsync(newProfile, ct);
        return newProfile;
    }

    public Task<Profile?> GetProfileWithUserByIdAsync(Guid profileId, CancellationToken ct = default)
        => context.Profiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == profileId, ct);
}
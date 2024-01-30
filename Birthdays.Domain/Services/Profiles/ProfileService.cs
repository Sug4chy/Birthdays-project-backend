using Data.Context;
using Data.Entities;

namespace Domain.Services.Profiles;

public class ProfileService(AppDbContext context) : IProfileService
{
    public async Task<Profile> CreateAsync(CancellationToken ct = default)
    {
        var newProfile = new Profile();
        await context.Profiles.AddAsync(newProfile, ct);
        return newProfile;
    }
}
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
    
    public Task<Profile?> GetProfileByIdAsync(Guid profileId, CancellationToken ct = default)
        => context.Profiles.FirstOrDefaultAsync(p => p.Id == profileId, ct);

    public Task<bool> CheckIfProfileExistsAsync(Guid profileId, CancellationToken ct = default)
        => context.Profiles.AnyAsync(p => p.Id == profileId, ct);
}
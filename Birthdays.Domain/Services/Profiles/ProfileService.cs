using Data.Entities;
using Data.Repositories;

namespace Domain.Services.Profiles;

public class ProfileService(IRepository<Profile> profilesDb) : IProfileService
{
    public async Task<Profile> CreateAsync(CancellationToken ct = default)
    {
        var newProfile = new Profile();
        await profilesDb.AddAsync(newProfile, ct);
        return newProfile;
    }
}
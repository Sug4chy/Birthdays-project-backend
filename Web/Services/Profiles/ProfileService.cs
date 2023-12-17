using Data.Entities;
using Data.Repositories;

namespace Web.Services.Profiles;

public class ProfileService(IRepository<Profile> profilesDb) : IProfileService
{
    public async Task<Profile> CreateAsync(CancellationToken ct = default)
    {
        var newProfile = new Profile();
        await profilesDb.CreateAndSaveAsync(newProfile, ct);
        return newProfile;
    }

    public Task CommitAsync(CancellationToken ct = default)
        => profilesDb.CommitChangesAsync(ct);
}
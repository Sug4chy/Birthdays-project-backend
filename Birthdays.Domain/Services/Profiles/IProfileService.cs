using Data.Entities;

namespace Domain.Services.Profiles;

public interface IProfileService
{
    Task<Profile> CreateAsync(CancellationToken ct = default);
    Task<Profile?> GetProfileWithWishesByIdAsync(Guid profileId, CancellationToken ct = default);
    Task<Profile?> GetProfileByIdAsync(Guid profileId, CancellationToken ct = default);
    Task<bool> CheckIfUserExistsAsync(Guid profileId, CancellationToken ct = default);
}
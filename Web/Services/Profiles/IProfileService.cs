using Data.Entities;

namespace Web.Services.Profiles;

public interface IProfileService
{
    Task<Profile> CreateAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
}
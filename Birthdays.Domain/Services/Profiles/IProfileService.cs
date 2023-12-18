using Data.Entities;

namespace Domain.Services.Profiles;

public interface IProfileService
{
    Task<Profile> CreateAsync(CancellationToken ct = default);
}
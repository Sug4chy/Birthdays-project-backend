using Data.Entities;

namespace Domain.Accessors;

public interface ICurrentUserAccessor
{
    Task<User> GetCurrentUserAsync(CancellationToken ct = default);
    Task<string> GetCurrentUserEmailAsync(CancellationToken ct = default);
}
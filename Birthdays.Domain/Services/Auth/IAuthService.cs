using Data.Entities;
using Domain.Models;

namespace Domain.Services.Auth;

public interface IAuthService
{
    Task<Error?> RegisterUserAsync(RegisterModel model, CancellationToken ct = default);
    ValueTask<string> GenerateToken(User user, CancellationToken ct = default);
}
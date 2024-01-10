using Data.Entities;
using Domain.Models;

namespace Domain.Services.Auth;

public interface IAuthService
{
    Task<Error?> RegisterUserAsync(RegisterModel model, CancellationToken ct = default);
    Task<Error?> LoginUserAsync(LoginModel model, CancellationToken ct = default);
    Task LogoutUserAsync(CancellationToken ct = default);
    ValueTask<string> GenerateToken(User user, CancellationToken ct = default);
}
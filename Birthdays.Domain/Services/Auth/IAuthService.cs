using Data.Entities;
using Domain.Models;

namespace Domain.Services.Auth;

public interface IAuthService
{
    Task<Error?> RegisterUserAsync(RegisterModel model, CancellationToken ct = default);
    Task<Error?> LoginUserAsync(LoginModel model, CancellationToken ct = default);
    Task<Error?> LogoutUserAsync(User user, CancellationToken ct = default);
    Task<string> GenerateToken(User user, CancellationToken ct = default);
}
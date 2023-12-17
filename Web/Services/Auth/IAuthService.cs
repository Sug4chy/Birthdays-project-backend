using Data.Entities;
using Web.Models;

namespace Web.Services.Auth;

public interface IAuthService
{
    Task RegisterUserAsync(RegisterModel model, CancellationToken ct = default);
    ValueTask<string> HashPassword(string password, CancellationToken ct = default);
    ValueTask<string> GenerateToken(User user, CancellationToken ct = default);
}
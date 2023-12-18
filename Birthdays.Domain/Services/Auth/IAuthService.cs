using Data.Entities;
using Domain.Models;

namespace Domain.Services.Auth;

public interface IAuthService
{
    Task RegisterUserAsync(RegisterModel model, CancellationToken ct = default);
    ValueTask<string> HashPassword(string password, CancellationToken ct = default);
    ValueTask<string> GenerateToken(User user, CancellationToken ct = default);
}
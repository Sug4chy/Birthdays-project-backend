using Data.Entities;
using Domain.Models;

namespace Domain.Services.Auth;

public interface IAuthService
{
    Task<string> RegisterUserAsync(RegisterModel model, CancellationToken ct = default);
    ValueTask<string> GenerateToken(User user, CancellationToken ct = default);
}
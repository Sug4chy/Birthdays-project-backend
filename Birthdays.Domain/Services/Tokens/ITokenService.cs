using System.Security.Claims;
using Data.Entities;

namespace Domain.Services.Tokens;

public interface ITokenService
{
    ValueTask<string> GenerateAccessToken(User user, CancellationToken ct = default);
    ValueTask<string> GenerateRefreshToken(CancellationToken ct = default);
    ValueTask<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token, CancellationToken ct = default);
}
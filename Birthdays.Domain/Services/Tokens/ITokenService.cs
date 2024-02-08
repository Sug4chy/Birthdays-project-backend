using System.Security.Claims;
using Data.Entities;
using Domain.Models;

namespace Domain.Services.Tokens;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    RefreshTokenModel GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    Claim[] GetClaimsFromJwt(string token);
}
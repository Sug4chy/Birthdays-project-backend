using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Data.Entities;
using Domain.Exceptions;
using Domain.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services.Tokens;

public class TokenService(IConfiguration config) : ITokenService
{
    public ValueTask<string> GenerateAccessToken(User user, CancellationToken ct = default)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Surname, user.Surname),
            new(ClaimTypes.Email, user.Email!)
        };
        
        var jwt = new JwtSecurityToken(
            issuer: config.GetValue<string>("Issuer"),
            audience: config.GetValue<string>("Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(config.GetValue<int>("AccessTokenExpiresTime")),
            signingCredentials: new SigningCredentials(
                GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return new ValueTask<string>(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    public ValueTask<string> GenerateRefreshToken(CancellationToken ct = default)
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return new ValueTask<string>(Convert.ToBase64String(randomNumber));
    }

    public ValueTask<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token, CancellationToken ct = default)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config.GetValue<string>("Issuer"),
            ValidateAudience = true,
            ValidAudience = config.GetValue<string>("Audience"),
            ValidateLifetime = true,
            IssuerSigningKey = GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        
        if (securityToken is not JwtSecurityToken jwtSecurityToken 
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new CustomValidationException(new Error("InvalidToken", "Invalid access token"));
        }
        
        return new ValueTask<ClaimsPrincipal>(principal);
    }

    private SymmetricSecurityKey GetSymmetricSecurityKey()
        => new(
            Encoding.UTF8.GetBytes(config.GetValue<string>("SymmetricSecurityKey")!
            ));
}
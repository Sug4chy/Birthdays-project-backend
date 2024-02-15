using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Data.Entities;
using Domain.Configs;
using Domain.Exceptions;
using Domain.Models;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Domain.Services.Tokens;

public class TokenService(IOptions<JwtConfigurationOptions> options) : ITokenService
{
    private readonly JwtConfigurationOptions _jwtConfigurationOptions = options.Value;
    
    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Surname, user.Surname),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        
        var jwt = new JwtSecurityToken(
            issuer: _jwtConfigurationOptions.Issuer,
            audience: _jwtConfigurationOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfigurationOptions.AccessTokenExpiresTimeInMinutes),
            signingCredentials: new SigningCredentials(
                GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public RefreshTokenModel GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return new RefreshTokenModel
        {
            Token = Convert.ToBase64String(randomNumber),
            TokenExpiryTime = DateTime.UtcNow
                .AddDays(_jwtConfigurationOptions.RefreshTokenExpiresTimeInDays)
        };
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtConfigurationOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtConfigurationOptions.Audience,
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

            BadRequestException.ThrowByValidationResult(new ValidationResult
            {
                Errors = [new ValidationFailure
                {
                    ErrorCode = "InvalidToken", 
                    ErrorMessage = "Invalid access token"
                }]
            });
        }
        
        return principal;
    }

    public Claim[] GetClaimsFromJwt(string token)
        => new JwtSecurityTokenHandler()
            .ReadJwtToken(token)
            .Claims
            .ToArray();

    private SymmetricSecurityKey GetSymmetricSecurityKey()
        => new(
            Encoding.UTF8.GetBytes(_jwtConfigurationOptions.SymmetricSecurityKey));
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services.Auth;

public class AuthService(UserManager<User> userManager, 
    SignInManager<User> signInManager, IConfiguration config) : IAuthService
{
    public async Task<string> RegisterUserAsync(RegisterModel model, CancellationToken ct = default)
    {
        var result = await userManager.CreateAsync(model.User, model.Password);
        if (!result.Succeeded)
        {
            return result.Errors.First().Code;
        }

        await signInManager.SignInAsync(model.User, false);
        return "";
    }

    public ValueTask<string> GenerateToken(User user, CancellationToken ct = default)
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
            expires: DateTime.UtcNow.AddMinutes(config.GetValue<int>("TokenExpiresTime")),
            signingCredentials: new SigningCredentials(
                GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return new ValueTask<string>(new JwtSecurityTokenHandler().WriteToken(jwt));
    }
    
    private SymmetricSecurityKey GetSymmetricSecurityKey()
        => new(
            Encoding.UTF8.GetBytes(config.GetValue<string>("SymmetricSecurityKey")!
            ));
}
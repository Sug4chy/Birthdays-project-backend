using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Entities;
using Data.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services.Auth;

public class AuthService(UserManager<User> userManager, 
    SignInManager<User> signInManager, IRepository<User> userRepo, IConfiguration config) : IAuthService
{
    public async Task<Error?> RegisterUserAsync(RegisterModel model, CancellationToken ct = default)
    {
        var result = await userManager.CreateAsync(model.User, model.Password);
        if (!result.Succeeded)
        {
            var identityError = result.Errors.First();
            return new Error { Code = identityError.Code, Message = identityError.Description };
        }

        await signInManager.SignInAsync(model.User, false);
        return null;
    }

    public async Task<Error?> LoginUserAsync(LoginModel model, CancellationToken ct = default)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email,
            model.Password, false, false);
        if (!result.Succeeded)
        {
            return new Error 
            { 
                Code = "LoginOrPasswordInvalid", 
                Message = "Login or/and password is/are not valid" 
            };
        }

        return null;
    }

    public async Task<Error?> LogoutUserAsync(User user, CancellationToken ct = default)
    {
        if (user.CurrentAccessToken is null)
        {
            return new Error
                { Code = "Unauthenticated", Message = $"User with email {user.Email} is not authenticated" };
        }
        
        await signInManager.SignOutAsync();
        user.CurrentAccessToken = null;
        await userRepo.Update(user);
        await userRepo.CommitChangesAsync(ct);
        return null;
    }

    public async Task<string> GenerateToken(User user, CancellationToken ct = default)
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

        string? token = new JwtSecurityTokenHandler().WriteToken(jwt);
        user.CurrentAccessToken = token;
        await userRepo.Update(user);
        await userRepo.CommitChangesAsync(ct);
        
        return token;
    }
    
    private SymmetricSecurityKey GetSymmetricSecurityKey()
        => new(
            Encoding.UTF8.GetBytes(config.GetValue<string>("SymmetricSecurityKey")!
            ));
}
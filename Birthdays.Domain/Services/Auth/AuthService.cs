using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Entities;
using Data.Repositories;
using Domain.Models;
using Domain.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services.Auth;

public class AuthService(UserManager<User> userManager, 
    SignInManager<User> signInManager, IRepository<User> userRepo, IConfiguration config) : IAuthService
{
    public async Task<Result> RegisterUserAsync(RegisterModel model, CancellationToken ct = default)
    {
        var result = await userManager.CreateAsync(model.User, model.Password);
        if (!result.Succeeded)
        {
            var identityError = result.Errors.First();
            return Result.FromIdentityError(identityError);
        }

        await signInManager.SignInAsync(model.User, false);
        return Result.Success();
    }

    public async Task<Result> LoginUserAsync(LoginModel model, CancellationToken ct = default)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email,
            model.Password, false, false);
        return result.Succeeded ? Result.Success() : Result.Failure(AuthErrors.SignInError);
    }

    public async Task<Result> LogoutUserAsync(User user, CancellationToken ct = default)
    {
        if (user.CurrentAccessToken is null)
        {
            return Result.Failure(AuthErrors.UnauthenticatedError(user.Email!));
        }
        
        await signInManager.SignOutAsync();
        user.CurrentAccessToken = null;
        await userRepo.Update(user);
        await userRepo.CommitChangesAsync(ct);
        return Result.Success();
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
using Data.Entities;
using Data.Repositories;
using Domain.Models;
using Domain.Results;
using Domain.Services.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Domain.Services.Auth;

public class AuthService(
    UserManager<User> userManager, 
    SignInManager<User> signInManager, 
    IRepository<User> userRepo,
    ITokenService tokenService) : IAuthService
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
        user.CurrentRefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.MinValue;
        await userRepo.CommitChangesAsync(ct);
        return Result.Success();
    }

    public string GiveUserRefreshToken(User user)
    {
        var refreshTokenModel = tokenService.GenerateRefreshToken();
        user.CurrentRefreshToken = refreshTokenModel.Token;
        user.RefreshTokenExpiryTime = refreshTokenModel.TokenExpiryTime;
        return refreshTokenModel.Token;
    }
}
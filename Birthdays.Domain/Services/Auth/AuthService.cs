using Data.Context;
using Data.Entities;
using Domain.Models;
using Domain.Results;
using Domain.Services.Tokens;
using Microsoft.AspNetCore.Identity;

namespace Domain.Services.Auth;

public class AuthService(
    UserManager<User> userManager, 
    SignInManager<User> signInManager,
    AppDbContext context,
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
        await context.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<AuthTokensModel> GenerateAndSetTokensAsync(User user, CancellationToken ct = default)
    {
        var refreshTokenModel = tokenService.GenerateRefreshToken();
        user.CurrentRefreshToken = refreshTokenModel.Token;
        user.RefreshTokenExpiryTime = refreshTokenModel.TokenExpiryTime;
        string accessToken = tokenService.GenerateAccessToken(user);
        await context.SaveChangesAsync(ct);
        return new AuthTokensModel { AccessToken = accessToken, RefreshToken = refreshTokenModel.Token };
    }
}
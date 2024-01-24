using Data.Entities;
using Domain.Models;
using Domain.Results;

namespace Domain.Services.Auth;

public interface IAuthService
{
    Task<Result> RegisterUserAsync(RegisterModel model, CancellationToken ct = default);
    Task<Result> LoginUserAsync(LoginModel model, CancellationToken ct = default);
    Task<Result> LogoutUserAsync(User user, CancellationToken ct = default);
    Task<AuthTokensModel> GenerateAndSetTokensAsync(User user, CancellationToken ct = default);
}
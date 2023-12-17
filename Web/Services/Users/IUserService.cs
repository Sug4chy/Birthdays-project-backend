using Data.Entities;
using Web.Handlers.Auth;

namespace Web.Services.Users;

public interface IUserService
{
    Task<User> CreateUserAsync(RegisterRequest request, 
        Profile profile, CancellationToken ct = default);
}
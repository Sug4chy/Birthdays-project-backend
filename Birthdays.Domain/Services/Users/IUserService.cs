using Data.Entities;
using Domain.Requests.Auth;

namespace Domain.Services.Users;

public interface IUserService
{
    Task<User> CreateUserAsync(RegisterRequest request, 
        Profile profile, CancellationToken ct = default);
}
using Data.Entities;
using Domain.DTO.Requests.Auth;

namespace Domain.Services.Users;

public interface IUserService
{
    Task<User> CreateUserAsync(RegisterRequest request, 
        Profile profile, CancellationToken ct = default);

    Task<User?> GetUserByEmail(string email, 
        CancellationToken ct = default);
}
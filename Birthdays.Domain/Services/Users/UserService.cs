using Data.Context;
using Data.Entities;
using Domain.DTO.Requests.Auth;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Users;

public class UserService(AppDbContext context) : IUserService
{
    public Task<User> CreateUserAsync(RegisterRequest request,
        Profile profile, CancellationToken ct = default)
        => Task.FromResult(new User
        {
            ProfileId = profile.Id,
            Profile = profile,
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic,
            UserName = request.Email,
            Email = request.Email,
            BirthDate = DateOnly.FromDateTime(request.BirthDate)
        });

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken ct = default)
        => context.Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<User?> GetUserByIdAsync(string id, CancellationToken ct = default)
        => context.Users
            .FirstOrDefaultAsync(u => u.Id == id, ct);
}
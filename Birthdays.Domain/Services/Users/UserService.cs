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
            BirthDate = new DateOnly(request.BirthDate.Year, request.BirthDate.Month, request.BirthDate.Day)
        });

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken ct = default)
        => context.Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<User?> GetUserByIdAsync(string id, CancellationToken ct = default)
        => context.Users
            .FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<List<User>> GetAllUsersWithPaginationIndexAsync(string currentUserId, int offset, int limit, 
        CancellationToken ct = default)
        => context.Users
            .Where(u => !u.Id.Equals(currentUserId))
            .Skip(limit * offset)
            .Take(limit)
            .ToListAsync(ct);

    public Task<User?> GetUserByTelegramChatIdAsync(long chatId, CancellationToken ct = default)
        => context.Users
            .FirstOrDefaultAsync(u => u.TelegramChatId == chatId, ct);

    public Task<User?> GetUserWithProfileByTelegramChatIdAsync(long chatId, CancellationToken ct = default)
        => context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.TelegramChatId == chatId, ct);
}
using Data.Entities;
using Domain.DTO.Requests.Auth;

namespace Domain.Services.Users;

public interface IUserService
{
    Task<User> CreateUserAsync(RegisterRequest request,
        Profile profile, CancellationToken ct = default);

    Task<User?> GetUserByEmailAsync(string email,
        CancellationToken ct = default);

    Task<User?> GetUserByIdAsync(string id, CancellationToken ct = default);

    Task<List<User>> GetAllUsersWithPaginationIndexAsync(string currentUserId, int offset, int limit,
        CancellationToken ct = default);

    Task<User?> GetUserByTelegramChatIdAsync(long chatId, CancellationToken ct = default);
}
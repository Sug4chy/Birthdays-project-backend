using Data.Entities;

namespace Domain.Services.Telegram;

public interface ITelegramService
{
    string GenerateLinkForUser(User user);
    Task SetChatIdToUserAsync(User user, long chatId, CancellationToken ct = default);
}
using Data.Context;
using Data.Entities;
using Domain.Configs;
using Microsoft.Extensions.Options;

namespace Domain.Services.Telegram;

public class TelegramService(
    IOptions<TgBotConfigurationOptions> options,
    AppDbContext context) : ITelegramService
{
    private readonly TgBotConfigurationOptions _tgBotConfiguration = options.Value;
    
    public string GenerateLinkForUser(User user)
        => $"{_tgBotConfiguration.BotLink}?start={user.Id}";

    public async Task SetChatIdToUserAsync(User user, long chatId, CancellationToken ct = default)
    {
        user.TelegramChatId = chatId;
        await Task.Run(() => context.Users.Update(user), ct);
        await context.SaveChangesAsync(ct);
    }
}
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Services;

public interface ITelegramUpdateListener
{
    public ITelegramBotClient? Client { get; set; }
    Task GetUpdateAsync(Update update, CancellationToken ct = default);
}
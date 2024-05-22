using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Commands;

public interface IBotCommand
{
    public string Name { get; }
    public ITelegramBotClient Client { get; }
    Task ExecuteAsync(Update update, CancellationToken ct = default);
}
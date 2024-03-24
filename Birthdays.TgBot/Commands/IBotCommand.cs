using Telegram.Bot.Types;

namespace Birthdays.TgBot.Commands;

public interface IBotCommand
{
    public string Name { get; }
    Task ExecuteAsync(Update update, CancellationToken ct = default);
}
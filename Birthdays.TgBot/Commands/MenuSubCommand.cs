using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Commands;

public class MenuSubCommand(
    ITelegramBotClient client) : IBotCommand
{
    public string Name => "/menu";

    public Task ExecuteAsync(Update update, CancellationToken ct = default)
        => new MenuCommand(client).ExecuteAsync(update, ct);
}
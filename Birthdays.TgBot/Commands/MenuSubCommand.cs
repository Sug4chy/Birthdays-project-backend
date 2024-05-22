using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Commands;

public class MenuSubCommand(
    Bot.Bot bot) : IBotCommand
{
    public string Name => "/menu";
    public ITelegramBotClient Client { get; } = bot.Client;

    public Task ExecuteAsync(Update update, CancellationToken ct = default)
        => new MenuCommand(bot).ExecuteAsync(update, ct);
}
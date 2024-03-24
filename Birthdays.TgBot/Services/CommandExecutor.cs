using Birthdays.TgBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Services;

public class CommandExecutor : ITelegramUpdateListener
{
    private IEnumerable<IBotCommand> Commands => [new StartCommand(Client!)];
    public ITelegramBotClient? Client { get; set; }

    public async Task GetUpdateAsync(Update update, CancellationToken ct = default)
    {
        var message = update.Message;
        if (message?.Text is null)
        {
            return;
        }

        foreach (var command in Commands
                     .Where(command => message.Text.StartsWith(command.Name)))
        {
            await command.ExecuteAsync(update, ct);
        }
    }
}
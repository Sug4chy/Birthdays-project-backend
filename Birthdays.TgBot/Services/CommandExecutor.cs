using Birthdays.TgBot.Commands;
using Birthdays.TgBot.Services.ServiceManager;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Services;

public class CommandExecutor : ITelegramUpdateListener
{
    private IEnumerable<IBotCommand> Commands =>
    [
        new StartCommand(Client!, ServiceManager!.UserService, ServiceManager!.TelegramService)
    ];

    public ITelegramBotClient? Client { get; set; }
    public IServiceManager? ServiceManager { get; set; }

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
            try
            {
                await command.ExecuteAsync(update, ct);
            }
            catch (Exception ex)
            {
                await Client!.SendTextMessageAsync(
                    update.Message!.Chat.Id,
                    ex.Message,
                    cancellationToken: ct
                );
            }
        }
    }
}
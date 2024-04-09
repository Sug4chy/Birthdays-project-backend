using Birthdays.TgBot.CallbackHandlers;
using Birthdays.TgBot.Commands;
using Birthdays.TgBot.Services.ServiceManager;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Birthdays.TgBot.Services;

public class CommandExecutor : ITelegramUpdateListener
{
    private IEnumerable<IBotCommand> Commands =>
    [
        new StartCommand(Client!, ServiceManager!.UserService, ServiceManager!.TelegramService),
        new MenuCommand(Client!),
        new MenuSubCommand(Client!),
        new MySubscriptionsCommand(Client!, ServiceManager!.UserService, ServiceManager!.SubscriptionsService)
    ];

    private IEnumerable<ICallbackHandler> CallbackHandlers =>
    [
        new SubscriptionsPaginationCallbackHandler(Client!, ServiceManager!.UserService,
            ServiceManager!.SubscriptionsService)
    ];

    public ITelegramBotClient? Client { get; set; }
    public IServiceManager? ServiceManager { get; set; }

    public async Task GetUpdateAsync(Update update, CancellationToken ct = default)
    {
        var message = update.Message;
        if (message?.Text is null && update.Type != UpdateType.CallbackQuery)
        {
            return;
        }

        switch (update.Type)
        {
            case UpdateType.CallbackQuery:
                await SafeExecuteCallbackAsync(update, ct);
                break;
            default:
                await SafeExecuteCommandAsync(update, ct);
                break;
        }
    }

    private async Task SafeExecuteCommandAsync(Update update, CancellationToken ct = default)
    {
        var message = update.Message;
        foreach (var command in Commands
                     .Where(command => message!.Text!.StartsWith(command.Name)))
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

    private async Task SafeExecuteCallbackAsync(Update update, CancellationToken ct = default)
    {
        string? callbackData = update.CallbackQuery!.Data;
        foreach (var callbackHandler in CallbackHandlers
                     .Where(handler => callbackData!.StartsWith(handler.Name)))
        {
            try
            {
                await callbackHandler.HandleCallbackAsync(update.CallbackQuery, ct);
            }
            catch (Exception e)
            {
                await Client!.SendTextMessageAsync(
                    update.CallbackQuery.Message!.Chat.Id,
                    e.Message,
                    cancellationToken: ct
                );
            }
        }
    }
}
using Birthdays.TgBot.CallbackHandlers;
using Birthdays.TgBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Birthdays.TgBot.Services;

public class UpdateDistributor(
    Bot.Bot bot,
    IServiceProvider serviceProvider
)
{
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
        var commands = serviceProvider.GetServices<IBotCommand>();
        foreach (var command in commands
                     .Where(command => message!.Text!.StartsWith(command.Name)))
        {
            try
            {
                await command.ExecuteAsync(update, ct);
            }
            catch (Exception ex)
            {
                await bot.Client.SendTextMessageAsync(
                    update.Message!.Chat.Id,
                    ex.Message,
                    cancellationToken: ct
                );
            }
        }
    }

    private async Task SafeExecuteCallbackAsync(Update update, CancellationToken ct = default)
    {
        var callbackHandlers = serviceProvider.GetServices<ICallbackHandler>();
        string? callbackData = update.CallbackQuery!.Data;
        foreach (var callbackHandler in callbackHandlers
                     .Where(handler => callbackData!.StartsWith(handler.Name)))
        {
            try
            {
                await callbackHandler.HandleCallbackAsync(update.CallbackQuery, ct);
            }
            catch (Exception e)
            {
                await bot.Client.SendTextMessageAsync(
                    update.CallbackQuery.Message!.Chat.Id,
                    e.Message,
                    cancellationToken: ct
                );
            }
        }
    }
}
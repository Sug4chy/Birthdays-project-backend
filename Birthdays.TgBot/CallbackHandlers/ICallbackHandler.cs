using Telegram.Bot.Types;

namespace Birthdays.TgBot.CallbackHandlers;

public interface ICallbackHandler
{
    public string Name { get; }
    Task HandleCallbackAsync(CallbackQuery? callback, CancellationToken ct = default);
}
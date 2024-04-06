using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Commands;

public class MenuCommand(
    ITelegramBotClient client) : IBotCommand
{
    public string Name => "Перейти в меню";

    public async Task ExecuteAsync(Update update, CancellationToken ct = default)
    {
        await client.SendTextMessageAsync(update.Message!.Chat.Id, "Тут будет меню.",
            cancellationToken: ct);
    }
}
using Domain.Services.Telegram;
using Domain.Services.Users;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Commands;

public class StartCommand(
    ITelegramBotClient client,
    IUserService userService,
    ITelegramService telegramService) : IBotCommand
{
    public string Name => "/start";
    
    public async Task ExecuteAsync(Update update, CancellationToken ct = default)
    {
        long chatId = update.Message!.Chat.Id;
        string[] messageParts = update.Message.Text!.Split(' ');
        if (messageParts.Length == 1)
        {
            await client.SendTextMessageAsync(chatId, "Друг, ну ты ключ-то где потерял?", 
                cancellationToken: ct);
            return;
        }

        string uniqueCode = messageParts[1];
        if (!Guid.TryParse(uniqueCode, out _))
        {
            await client.SendTextMessageAsync(chatId, "Невалидный у тебя код, дружочек", 
                cancellationToken: ct);
            return;
        }

        var user = await userService.GetUserByIdAsync(uniqueCode, ct);
        if (user is null)
        {
            await client.SendTextMessageAsync(chatId, "Лол", cancellationToken: ct);
            return;
        }

        if (user.TelegramChatId != 0)
        {
            await client.SendTextMessageAsync(chatId,
                $"Ну здравствуй {user.Name}, будь как дома.", 
                cancellationToken: ct);
            return;
        }

        await telegramService.SetChatIdToUserAsync(user, chatId, ct);
        await client.SendTextMessageAsync(chatId,
            $"Ну здравствуй дорогой, будь как дома. Кстати, твой ключ: {uniqueCode}", 
            cancellationToken: ct);
    }
}
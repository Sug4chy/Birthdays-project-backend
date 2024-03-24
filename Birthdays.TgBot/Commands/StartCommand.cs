using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Commands;

public class StartCommand(ITelegramBotClient client) : IBotCommand
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
        await client.SendTextMessageAsync(chatId,
            $"Ну здравствуй дорогой, будь как дома. Кстати, твой ключ: {uniqueCode}", 
            cancellationToken: ct);
    }
}
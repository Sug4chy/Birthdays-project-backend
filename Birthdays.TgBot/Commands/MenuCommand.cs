using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Birthdays.TgBot.Commands;

public class MenuCommand(
    ITelegramBotClient client) : IBotCommand
{
    public string Name => "Перейти в меню";
    
    private static ReplyKeyboardMarkup KeyboardMarkup =>
        new(
        [
            [
                new KeyboardButton("Мой профиль"),
                new KeyboardButton("Мои подписки")
            ]
        ])
        {
            ResizeKeyboard = true
        };

    public async Task ExecuteAsync(Update update, CancellationToken ct = default)
    {
        const string text = """
                            Добро пожаловать в меню бота "Тинькофф Именины"!
                            Здесь вы можете:
                             - просмотреть список тех, на кого вы подписаны, а также перейти к просмотру их профилей;
                             - просмотреть свой профиль в том виде, как его видят другие люди.
                            Для выполнения этих операций вы можете воспользоваться кнопками, которые расположены на вашей клавиатуре.
                            Если захотите вернуться сюда, пропишите команду /menu.
                            """;
        await client.SendTextMessageAsync(update.Message!.Chat.Id, text,
            replyMarkup: KeyboardMarkup, cancellationToken: ct);
    }
}
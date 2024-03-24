using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Birthdays.TgBot.Bot;

public class Bot()
{
    public required ITelegramBotClient Client { get; init; }

    public Bot(IOptions<BotConfigOptions> options) : this()
    {
        Client = new TelegramBotClient(options.Value.Key);
    }
}
using System.Text;
using Birthdays.TgBot.Exceptions;
using Data.Entities;
using Domain.Services.Users;
using Domain.Services.WishLists;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Commands;

public class MyProfileCommand(
    Bot.Bot bot,
    IUserService userService,
    IWishListService wishListService
) : IBotCommand
{
    private static readonly string[] ErrorMessages =
    [
        "Я вас не узнаю, поэтому и не могу сказать, подписаны ли вы хоть на кого-то. Извините"
    ];
    
    private enum Errors
    {
        NoSuchUser
    }
    
    public string Name => "Мой профиль";
    public ITelegramBotClient Client { get; } = bot.Client;

    public async Task ExecuteAsync(Update update, CancellationToken ct = default)
    {
        if (update.Message is null)
        {
            return;
        }

        long chatId = update.Message.Chat.Id;
        var user = await userService.GetUserWithProfileByTelegramChatIdAsync(chatId, ct);
        TgBotException.ThrowIf(user is null, ErrorMessages[(int)Errors.NoSuchUser]);

        var wishLists = await wishListService.GetWishListsByProfileIdAsync(user!.ProfileId, ct);

        string patronymic = user.Patronymic is not null 
            ? $"Отчество: {user.Patronymic}" 
            : "";
        var sb = new StringBuilder();
        sb.AppendLine($"""
                       Пожалуйста, вот ваш профиль:

                       Login: {user.UserName}
                        
                       Имя: {user.Name}
                       Фамилия: {user.Surname}
                       {patronymic}
                       """);
        if (wishLists.Count == 0)
        {
            await Client.SendTextMessageAsync(chatId, sb.ToString(), cancellationToken: ct);
            return;
        }

        sb.AppendLine("Списки пожеланий:");
        for (int i = 0; i < wishLists.Count; i++)
        {
            wishLists[i].Wishes ??= new List<Wish>();
            string description = wishLists[i].Description is not null
                ? $"{wishLists[i].Description}"
                : "";
            sb.AppendLine($"""
                           {i + 1}. {wishLists[i].Name}
                           {description}
                           """);
            foreach (var wish in wishLists[i].Wishes!)
            {
                string wishDescription = wish.Description is not null
                    ? $"\n   {wish.Description}"
                    : "";
                string giftRef = wish.GiftRef is not null
                    ? $" ({wish.GiftRef})"
                    : "";
                sb.AppendLine($" - {wish.Name}{wishDescription}{giftRef}");
            }

            sb.AppendLine();
        }

        await Client.SendTextMessageAsync(chatId, sb.ToString(), cancellationToken: ct);
    }
}
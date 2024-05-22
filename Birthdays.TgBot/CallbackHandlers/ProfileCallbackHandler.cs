using System.Text;
using Birthdays.TgBot.Exceptions;
using Data.Entities;
using Domain.Services.Users;
using Domain.Services.WishLists;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Birthdays.TgBot.CallbackHandlers;

public class ProfileCallbackHandler(
    Bot.Bot bot,
    IUserService userService,
    IWishListService wishListService
) : ICallbackHandler
{
    private static readonly string[] ErrorMessages =
    [
        "Извините, что-то пошло не так. Не могу найти пользователя с нулевым кодом...",
        "Извините, что-то пошло не так. Не могу найти пользователя с таким кодом..."
    ];

    private enum Errors
    {
        NoProfileId,
        NoSuchUser
    }

    private ITelegramBotClient Client { get; } = bot.Client;

    public string Name => "profile";

    public async Task HandleCallbackAsync(CallbackQuery? callback, CancellationToken ct = default)
    {
        long chatId = callback!.Message!.Chat.Id;
        string[] callbackParts = callback.Data?.Split(' ') ?? [];
        TgBotException.ThrowIf(callbackParts.Length <= 1, ErrorMessages[(int)Errors.NoProfileId]);

        var user = await userService.GetUserByIdAsync(callbackParts[1], ct);
        TgBotException.ThrowIf(user is null, ErrorMessages[(int)Errors.NoSuchUser]);

        var wishLists = await wishListService.GetWishListsByProfileIdAsync(user!.ProfileId, ct);

        string patronymic = user.Patronymic is not null
            ? $"Отчество: {user.Patronymic}"
            : "";
        var sb = new StringBuilder();
        sb.AppendLine($"""
                       Вот, профиль пользователя {user.UserName}:

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
                sb.AppendLine($" - [{wish.Name}]({wish.GiftRef}){wishDescription}");
            }

            sb.AppendLine();
        }

        await Client.SendTextMessageAsync(chatId, sb.ToString(),
            parseMode: ParseMode.Html,
            cancellationToken: ct
        );
    }
}
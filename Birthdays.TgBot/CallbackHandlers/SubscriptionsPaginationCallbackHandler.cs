using System.Text;
using Birthdays.TgBot.Exceptions;
using Domain.Services.Subscriptions;
using Domain.Services.Users;
using Microsoft.IdentityModel.Tokens;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Birthdays.TgBot.CallbackHandlers;

public class SubscriptionsPaginationCallbackHandler(
    Bot.Bot bot,
    IUserService userService,
    ISubscriptionsService subscriptionsService) : ICallbackHandler
{
    private static readonly string[] ErrorMessages =
    [
        "Я вас не узнаю, поэтому и не могу сказать, подписаны ли вы хоть на кого-то. Извините",
        "Кажется, вы пока ни на кого не подписаны, поэтому мне нечего вам здесь показать. Извините"
    ];

    private enum Errors
    {
        NoSuchUser,
        NoSubscriptions
    }

    public string Name => "subscriptions";

    public async Task HandleCallbackAsync(CallbackQuery? callback, CancellationToken ct = default)
    {
        long chatId = callback!.Message!.Chat.Id;
        var user = await userService.GetUserByTelegramChatIdAsync(chatId, ct);
        TgBotException.ThrowIf(user is null, ErrorMessages[(int)Errors.NoSuchUser]);

        string[] textParts = callback.Data!.Split(' ');
        int pageIndex = textParts.Length == 1
            ? 0
            : int.Parse(textParts[1]);

        var subscriptions = await subscriptionsService
            .GetSubscriptionsByProfileIdWithPaginationAsync(user!.ProfileId, pageIndex, ct);
        TgBotException.ThrowIf(subscriptions.IsNullOrEmpty() && pageIndex == 0, 
            ErrorMessages[(int)Errors.NoSubscriptions]);
        var keyboard = GenerateKeyboard(pageIndex);
        if (subscriptions.IsNullOrEmpty())
        {
            await bot.Client.DeleteMessageAsync(chatId, callback.Message.MessageId, ct);
            await bot.Client.SendTextMessageAsync(chatId, "Эта страница пуста! " +
                                                      "Пожалуйста, вернитесь на предыдущую",
                replyMarkup: keyboard, cancellationToken: ct);
            return;
        }

        var sb = new StringBuilder();
        for (int i = 0; i < subscriptions.Length; i++)
        {
            var tempUser = subscriptions[i].BirthdayMan!.User!;
            string patronymic = tempUser.Patronymic is null
                ? $" {tempUser.Patronymic}"
                : "";
            sb.AppendLine($"{i + 1}) {tempUser.Name}" +
                          $" {tempUser.Name}{patronymic}: " +
                          $"{tempUser.BirthDate.ToShortDateString()}");
        }
        
        await bot.Client.SendTextMessageAsync(chatId, sb.ToString(),
            replyMarkup: keyboard, cancellationToken: ct);
        await bot.Client.DeleteMessageAsync(chatId, callback.Message.MessageId, ct);
    }

    private static InlineKeyboardMarkup GenerateKeyboard(int pageIndex)
    {
        string prevIndex = pageIndex <= 1 ? "" : $" {pageIndex - 1}";
        string nextIndex = $" {pageIndex + 1}";
        return new InlineKeyboardMarkup(
        [
            [
                new InlineKeyboardButton("Предыдущая страница")
                {
                    CallbackData = $"subscriptions{prevIndex}"
                },
                new InlineKeyboardButton("Следующая страница")
                {
                    CallbackData = $"subscriptions{nextIndex}"
                }
            ]
        ]);
    }
}
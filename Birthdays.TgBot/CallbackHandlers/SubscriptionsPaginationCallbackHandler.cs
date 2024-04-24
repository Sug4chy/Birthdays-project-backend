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
        if (subscriptions.IsNullOrEmpty())
        {
            await bot.Client.SendTextMessageAsync(chatId, "Эта страница пуста! " +
                                                      "Пожалуйста, вернитесь на предыдущую",
                replyMarkup: GenerateKeyboard(pageIndex, []), cancellationToken: ct);
            await bot.Client.DeleteMessageAsync(chatId, callback.Message.MessageId, ct);
            return;
        }

        var sb = new StringBuilder();
        var buttons = new List<InlineKeyboardButton>();
        for (int i = 0; i < subscriptions.Length; i++)
        {
            var tempUser = subscriptions[i].BirthdayMan!.User!;
            string patronymic = tempUser.Patronymic is null
                ? $" {tempUser.Patronymic}"
                : "";
            sb.AppendLine($"{i + 1}) {tempUser.Name}" +
                          $" {tempUser.Name}{patronymic}: " +
                          $"{tempUser.BirthDate.ToShortDateString()}");
            buttons.Add(new InlineKeyboardButton(tempUser.UserName!)
            {
                CallbackData = $"profile {tempUser.Id}"
            });
        }
        
        await bot.Client.SendTextMessageAsync(chatId, sb.ToString(),
            replyMarkup: GenerateKeyboard(pageIndex, buttons), cancellationToken: ct);
        await bot.Client.DeleteMessageAsync(chatId, callback.Message.MessageId, ct);
    }

    private static InlineKeyboardMarkup GenerateKeyboard(int pageIndex, List<InlineKeyboardButton> buttons)
    {
        string prevIndex = pageIndex <= 1 ? "" : $" {pageIndex - 1}";
        string nextIndex = $" {pageIndex + 1}";
        var firstRowButtons = buttons.Count >= 3 
            ? buttons[..3] 
            : buttons;
        var secondRowButtons = buttons.Count >= 5 
            ? buttons[3..5] 
            : buttons.Count >= 3 
                ? buttons[3..]
                : [];
        var thirdRowButtons = buttons.Count > 5
            ? buttons[5..]
            : [];
        return new InlineKeyboardMarkup(
        [
            firstRowButtons,
            secondRowButtons,
            thirdRowButtons,
            [
                new InlineKeyboardButton("<=")
                {
                    CallbackData = $"subscriptions{prevIndex}"
                },
                new InlineKeyboardButton("=>")
                {
                    CallbackData = $"subscriptions{nextIndex}"
                }
            ]
        ]);
    }
}
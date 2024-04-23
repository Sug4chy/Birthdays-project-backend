﻿using System.Text;
using Birthdays.TgBot.Exceptions;
using Domain.Services.Subscriptions;
using Domain.Services.Users;
using Microsoft.IdentityModel.Tokens;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Birthdays.TgBot.Commands;

public class MySubscriptionsCommand(
    Bot.Bot bot,
    IUserService userService,
    ISubscriptionsService subscriptionsService) : IBotCommand
{
    public string Name => "Мои подписки";
    public ITelegramBotClient Client { get; } = bot.Client;

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

    private static readonly IReplyMarkup Keyboard = new InlineKeyboardMarkup(
    [
        [
            new InlineKeyboardButton("Предыдущая страница") { CallbackData = "subscriptions" },
            new InlineKeyboardButton("Следующая страница") { CallbackData = "subscriptions 1" }
        ]
    ]);

    public async Task ExecuteAsync(Update update, CancellationToken ct = default)
    {
        if (update.Message is null)
        {
            return;
        }

        long chatId = update.Message.Chat.Id;
        var user = await userService.GetUserWithProfileByTelegramChatIdAsync(chatId, ct);
        TgBotException.ThrowIf(user is null, ErrorMessages[(int)Errors.NoSuchUser]);

        var subscriptions = await subscriptionsService
            .GetSubscriptionsByProfileIdWithPaginationAsync(user!.ProfileId, 0, ct);
        TgBotException.ThrowIf(subscriptions.IsNullOrEmpty(), ErrorMessages[(int)Errors.NoSubscriptions]);

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

        await Client.SendTextMessageAsync(chatId, sb.ToString(), 
            replyMarkup: Keyboard, cancellationToken: ct);
    }
}
﻿using Birthdays.TgBot.Exceptions;
using Domain.Services.Telegram;
using Domain.Services.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Birthdays.TgBot.Commands;

public class StartCommand(
    Bot.Bot bot,
    IUserService userService,
    ITelegramService telegramService) : IBotCommand
{
    public string Name => "/start";
    public ITelegramBotClient Client { get; } = bot.Client;

    private static readonly string[] ErrorTexts =
    [
        "Вижу, вы сюда попали случайно. Пожалуйста, зарегистрируйтесь",
        "Такие фокусы здесь не пройдут, авторизуйся всё же на нашем сайте",
        "Так, а тебя нет в БД. Ты точно зарегистрировался на сайте!?"
    ];

    private enum Error
    {
        NoUniqueCode,
        UniqueCodeIsNotGuid,
        NoUserWithThatId
    }

    private static ReplyKeyboardMarkup KeyboardMarkup =>
        new(
        [
            [
                new KeyboardButton("Перейти в меню")
            ]
        ])
        {
            ResizeKeyboard = true
        };

    public async Task ExecuteAsync(Update update, CancellationToken ct = default)
    {
        long chatId = update.Message!.Chat.Id;
        var user = await userService.GetUserByTelegramChatIdAsync(chatId, ct);
        if (user is not null)
        {
            await Client.SendTextMessageAsync(chatId,
                $"Здравствуйте, {user.UserName}! Вас приветствует бот проекта \"Тинькофф Именины\". " +
                "Пожалуйста, выберите команду в меню, и я дам вам то, что вы запросили",
                replyMarkup: KeyboardMarkup, cancellationToken: ct);
        }
        else
        {
            string[] messageParts = update.Message.Text!.Split(' ');
            TgBotException.ThrowIf(messageParts.Length == 1, ErrorTexts[(int)Error.NoUniqueCode]);
            
            string uniqueCode = messageParts[1];
            TgBotException.ThrowIf(!Guid.TryParse(uniqueCode, out _), 
                ErrorTexts[(int)Error.UniqueCodeIsNotGuid]);

            user = await userService.GetUserByIdAsync(uniqueCode, ct);
            TgBotException.ThrowIf(user is null, ErrorTexts[(int)Error.NoUserWithThatId]);
            
            await telegramService.SetChatIdToUserAsync(user!, chatId, ct);
            await Client.SendTextMessageAsync(chatId,
                $"Поздравляю с успешной регистрацией на нашем сайте, {user!.UserName}! " +
                "Вас приветствует бот проекта \"Тинькофф Именины\". " +
                "Пожалуйста, выберите команду в меню, и я дам вам то, что вы запросили",
                replyMarkup: KeyboardMarkup,
                cancellationToken: ct);
        }
    }
}
﻿using Telegram.Bot.Types;

namespace Birthdays.TgBot.Services;

public class UpdateDistributor<T>(Bot.Bot bot) where T : ITelegramUpdateListener, new()
{
    private readonly Dictionary<long, T> _listeners = new();

    public async Task GetUpdateAsync(Update update, CancellationToken ct = default)
    {
        long chatId = update.Message!.Chat.Id;
        var listener = _listeners.GetValueOrDefault(chatId);
        if (listener is null)
        {
            listener = new T
            {
                Client = bot.Client
            };
            _listeners.Add(chatId, listener);
        }
        
        await listener.GetUpdateAsync(update, ct);
    }
}
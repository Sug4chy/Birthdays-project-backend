using Birthdays.TgBot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Birthdays.TgBot.Controllers;

[ApiController]
[Route("/")]
public class BotController(UpdateDistributor updateDistributor) : ControllerBase
{
    [HttpPost]
    public async Task Update(Update update, CancellationToken ct = default)
    {
        if (update.Message is null && update.Type != UpdateType.CallbackQuery)
        {
            return;
        }

        await updateDistributor.GetUpdateAsync(update, ct);
    }
}
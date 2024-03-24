using Birthdays.TgBot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Birthdays.TgBot.Controllers;

[ApiController]
[Route("/")]
public class BotController(UpdateDistributor<CommandExecutor> updateDistributor) : ControllerBase
{
    [HttpPost]
    public async Task Update(Update update, CancellationToken ct = default)
    {
        if (update.Message is null)
        {
            return;
        }

        await updateDistributor.GetUpdateAsync(update, ct);
    }
}
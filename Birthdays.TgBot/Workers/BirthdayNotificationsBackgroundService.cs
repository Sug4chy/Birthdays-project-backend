using Birthdays.TgBot.Configs;
using Birthdays.TgBot.Services;
using Data.Context;
using Microsoft.Extensions.Options;

namespace Birthdays.TgBot.Workers;

public class BirthdayNotificationsBackgroundService(
    Bot.Bot bot,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<BirthdayNotificationsBackgroundService> logger,
    IOptions<NotificationConfigOptions> options
) : BackgroundService
{
    private readonly NotificationConfigOptions _notificationConfigOptions = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await new NotificationCheckerAndSender(bot, context, logger).CheckAndSendNotificationsAsync(
                stoppingToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{Message} error occured", e.Message);
        }

        using var timer = new PeriodicTimer(TimeSpan.FromDays(_notificationConfigOptions.IntervalDays));
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await new NotificationCheckerAndSender(bot, context, logger).CheckAndSendNotificationsAsync(
                        stoppingToken);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "{Message} error occured", e.Message);
                }
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Execution of {BackgroundService} background service was canceled",
                GetType().Name);
        }
    }
}
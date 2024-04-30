using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Birthdays.TgBot.Workers;

public class BirthdayNotificationsBackgroundService(
    Bot.Bot bot,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await CheckAndSendNotificationsAsync(context, stoppingToken);
        }

        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await CheckAndSendNotificationsAsync(context, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Oops");
        }
    }

    private async Task CheckAndSendNotificationsAsync(AppDbContext dbContext, CancellationToken ct = default)
    {
        int[] daysCounts = [0, 1, 7];
        var daysCountsAsStrings = new Dictionary<int, string>
        {
            { 0, "сегодня" },
            { 1, "завтра" },
            { 7, "через неделю" }
        };
        foreach (int daysCount in daysCounts)
        {
            var users = await GetUsersWithBirthdayAfterDaysCountAsync(dbContext, daysCount, ct);
            await SendNotificationsAsync(dbContext, users, daysCountsAsStrings[daysCount], ct);
        }
    }

    private async Task SendNotificationsAsync(AppDbContext dbContext, IEnumerable<User> users,
        string timeToBirthdayString, CancellationToken ct = default)
    {
        foreach (var birthdayMan in users)
        {
            var subscriptions = birthdayMan.Profile!.SubscriptionsAsBirthdayMan;
            if (subscriptions is null)
            {
                continue;
            }

            foreach (var subscription in subscriptions)
            {
                var subscriber = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.ProfileId == subscription.SubscriberId, ct);
                if (subscriber!.TelegramChatId == 0)
                {
                    continue;
                }

                await bot.Client.SendTextMessageAsync(subscriber.TelegramChatId,
                    $"Напоминаю вам, что у пользователя {birthdayMan.UserName}" +
                    $"({birthdayMan.Surname} {birthdayMan.Name}) {timeToBirthdayString} будет день рождения! " +
                    $"Пожалуйста, подготовьте подарок и не забудьте поздравить именинника",
                    cancellationToken: ct);
            }
        }
    }

    private static Task<List<User>> GetUsersWithBirthdayAfterDaysCountAsync(AppDbContext dbContext,
        int daysCount, CancellationToken ct = default)
        => dbContext.Users
            .Include(u => u.Profile)
            .ThenInclude(p => p!.SubscriptionsAsBirthdayMan)
            .Where(u => u.BirthDate.Month == DateTime.Today.Month 
                        && u.BirthDate.Day == DateTime.Today.Day + daysCount)
            .ToListAsync(ct);
}
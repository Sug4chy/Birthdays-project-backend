using Domain.Services.Telegram;
using Domain.Services.Users;

namespace Birthdays.TgBot.Services.ServiceManager;

public class ServiceManager(
    IUserService userService,
    ITelegramService telegramService) : IServiceManager
{
    public IUserService UserService { get; init; } = userService;
    public ITelegramService TelegramService { get; init; } = telegramService;
}
using Domain.Services.Telegram;
using Domain.Services.Users;

namespace Birthdays.TgBot.Services.ServiceManager;

public interface IServiceManager
{
    IUserService UserService { get; init; }
    ITelegramService TelegramService { get; init; }
}
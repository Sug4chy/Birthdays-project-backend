using Birthdays.TgBot.CallbackHandlers;
using Birthdays.TgBot.Commands;

namespace Birthdays.TgBot.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBotCommands(this IServiceCollection services)
        => services.AddScoped<IBotCommand, MenuCommand>()
            .AddScoped<IBotCommand, MenuSubCommand>()
            .AddScoped<IBotCommand, MySubscriptionsCommand>()
            .AddScoped<IBotCommand, StartCommand>();

    public static IServiceCollection AddCallbackHandlers(this IServiceCollection services)
        => services.AddScoped<ICallbackHandler, SubscriptionsPaginationCallbackHandler>();
}
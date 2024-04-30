using Birthdays.TgBot.Bot;
using Birthdays.TgBot.Extensions;
using Birthdays.TgBot.Services;
using Birthdays.TgBot.Workers;
using Data.Extensions;
using Domain.Services.Subscriptions;
using Domain.Services.Telegram;
using Domain.Services.Users;
using Domain.Services.WishLists;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BotConfigOptions>(builder.Configuration.GetSection("Bot"));
builder.Services.AddSingleton<Bot>();

builder.Services.AddDataLayerServices(builder.Configuration);
builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITelegramService, TelegramService>();
builder.Services.AddScoped<IWishListService, WishListService>();

builder.Services.AddHostedService<BirthdayNotificationsBackgroundService>();

builder.Services.AddBotCommands();
builder.Services.AddCallbackHandlers();

builder.Services.AddScoped<UpdateDistributor>();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

app.MapControllers();
app.Run();
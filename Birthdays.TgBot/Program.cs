using Birthdays.TgBot.Bot;
using Birthdays.TgBot.Services;
using Birthdays.TgBot.Services.ServiceManager;
using Data.Extensions;
using Domain.Services.Telegram;
using Domain.Services.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BotConfigOptions>(builder.Configuration.GetSection("Bot"));
builder.Services.AddSingleton<Bot>();

builder.Services.AddDataLayerServices(builder.Configuration);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITelegramService, TelegramService>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<UpdateDistributor<CommandExecutor>>();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

app.MapControllers();
app.Run();
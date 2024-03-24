using Birthdays.TgBot.Bot;
using Birthdays.TgBot.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BotConfigOptions>(builder.Configuration.GetSection("Bot"));
builder.Services.AddSingleton<Bot>();
builder.Services.AddScoped<UpdateDistributor<CommandExecutor>>();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

app.MapControllers();
app.Run();
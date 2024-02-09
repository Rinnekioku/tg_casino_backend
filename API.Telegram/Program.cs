using Telegram.Bot;
using API.Telegram.Services;
using API.Telegram.Services.EventDispatcher;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<EventDispatcherService>();
builder.Services.AddScoped<ReceiverService>();
builder.Services.AddHostedService<PollingService>();
builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient) =>
    {
        TelegramBotClientOptions options = new(builder.Configuration.GetSection("Telegram").GetSection("Token").Value!);
        return new TelegramBotClient(options, httpClient);
    });

var app = builder.Build();

app.Run();
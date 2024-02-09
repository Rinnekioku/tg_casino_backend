using Microsoft.Extensions.Configuration.UserSecrets;
using Common.Utils.Extensions;
using Common.Utils.Messaging.Events;
using Common.Utils.Messaging.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Worker.Telegram.Messaging.Handlers;
using Worker.Telegram.Services;
using Worker.Telegram.Services.TelegramWebhookDispatching;
using Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<TelegramWebhookDispatcher>();
builder.Services.AddScoped<ReceiverService>();
builder.Services.AddHostedService<PollingService>();
builder.Services.AddUtilityServices(builder.Configuration);
builder.Services.AddTransient<TelegramLoginResponseHandler>();
builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>(httpClient =>
    {
        TelegramBotClientOptions options = new(builder.Configuration["Telegram:Token"]!);
        return new TelegramBotClient(options, httpClient);
    });
builder.Services.AddScoped<CallbackQueryHandler>();
builder.Services.AddScoped<MessageHandler>();

var app = builder.Build();

var telegramWebhookDispatcher = app.Services.GetRequiredService<TelegramWebhookDispatcher>();

telegramWebhookDispatcher.On<CallbackQueryHandler>(UpdateType.CallbackQuery);
telegramWebhookDispatcher.On<MessageHandler>(UpdateType.Message);

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.On<TelegramLoginResponse, TelegramLoginResponseHandler>();

app.Run();
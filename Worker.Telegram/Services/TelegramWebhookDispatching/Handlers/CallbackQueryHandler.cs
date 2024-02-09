using Common.Utils.Messaging.Events;
using Common.Utils.Messaging.Interfaces;
using Worker.Telegram.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

public class CallbackQueryHandler : UpdateHandlerBase
{
    private readonly IEventBus _eventBus;
    
    public CallbackQueryHandler(ITelegramBotClient botClient, IEventBus eventBus) : base(botClient)
    {
        _eventBus = eventBus;
    }

    public override async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var query = update.CallbackQuery!;
        if (query.GameShortName != "aff_casino")
        {
            await BotClient.AnswerCallbackQueryAsync(
                callbackQueryId: query.Id,
                cancellationToken: cancellationToken,
                text: "Invalid command"
            );
        }
        else
        {
            await _eventBus.Publish(new TelegramLoginRequest { Username = update.CallbackQuery!.From.Username! });
            await BotClient.AnswerCallbackQueryAsync(
                callbackQueryId: query.Id,
                cancellationToken: cancellationToken,
                url: "https://affhub.ovh/"
            );
        }
    }
}
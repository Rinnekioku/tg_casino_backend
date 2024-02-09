using Worker.Telegram.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Worker.Telegram.Services.EventDispatcher.Handlers;

public class CallbackQueryHandler : EventHandlerBase
{
    public CallbackQueryHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task Process(Update update, CancellationToken cancellationToken)
    {
        var query = update.CallbackQuery!;
        if (query.GameShortName != "aff_casino")
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: query.Id,
                cancellationToken: cancellationToken,
                text: "Invalid command"
            );
        }
        else
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: query.Id,
                cancellationToken: cancellationToken,
                url: "https://affhub.ovh/"
            );
        }
    }
}
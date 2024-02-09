using API.Telegram.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace API.Telegram.Services.EventDispatcher.Handlers;

public class CallbackQueryHandler : EventHandlerBase
{
    public CallbackQueryHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task Process(Update update)
    {
        var query = update.CallbackQuery!;
        if (query.GameShortName != "aff_casino")
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: query.Id,
                text: "Invalid command"
            );
        }
        else
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQueryId: query.Id,
                url: "https://affhub.ovh/"
            );
        }
    }
}

/*
 * bot.on("callback_query", function (query) {
    if (query.game_short_name !== gameName) {
        bot.answerCallbackQuery(query.id, "Sorry, '" + query.game_short_name + "' is not available.");
    } else {
        queries[query.id] = query;
        let gameurl = "https://maxizhukov.github.io/telegram_game_front/";
        bot.answerCallbackQuery({
            callback_query_id: query.id,
            url: gameurl
        });
    }
});
 */
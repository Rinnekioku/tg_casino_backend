using Telegram.Bot;
using Telegram.Bot.Types;
using Worker.Telegram.Enums;

namespace Worker.Telegram.Services.EventDispatcher.Handlers;

public class MessageHandler : EventHandlerBase
{
    public MessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task Process(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message!;
        string messageEventString = message.Text!;
        if (messageEventString.StartsWith("/"))
        {
            messageEventString = messageEventString.Substring(1);
        }

        if (!Enum.TryParse(messageEventString, true, out BotMessageEvent messageEvent))
        {
            return;
        }

        switch (messageEvent)
        {
            case BotMessageEvent.Start:
            {
                await _botClient.SendGameAsync(
                    chatId: message.Chat.Id,
                    cancellationToken: cancellationToken,
                    gameShortName: "aff_casino");
                break;
            }
            case BotMessageEvent.Play:
            {
                await _botClient.SendGameAsync(
                    chatId: message.Chat.Id,
                    cancellationToken: cancellationToken,
                    gameShortName: "aff_casino");
                break;
            }
        }
    }
}
using Telegram.Bot;
using Telegram.Bot.Types;
using Worker.Telegram.Enums;

namespace Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

public class MessageHandler : UpdateHandlerBase
{
    public MessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task Handle(Update update, CancellationToken cancellationToken)
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
                await BotClient.SendGameAsync(
                    chatId: message.Chat.Id,
                    cancellationToken: cancellationToken,
                    gameShortName: "aff_casino");
                break;
            }
            case BotMessageEvent.Play:
            {
                await BotClient.SendGameAsync(
                    chatId: message.Chat.Id,
                    cancellationToken: cancellationToken,
                    gameShortName: "aff_casino");
                break;
            }
        }
    }
}
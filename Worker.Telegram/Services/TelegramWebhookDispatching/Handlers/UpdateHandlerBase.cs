using Worker.Telegram.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

public class UpdateHandlerBase : IUpdateHandler
{
    protected readonly ITelegramBotClient BotClient;

    internal UpdateHandlerBase(ITelegramBotClient botClient)
    {
        BotClient = botClient;
    }

    public virtual Task Handle(Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
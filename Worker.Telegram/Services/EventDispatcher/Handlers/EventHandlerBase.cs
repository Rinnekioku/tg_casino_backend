using Worker.Telegram.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Worker.Telegram.Services.EventDispatcher.Handlers;

public class EventHandlerBase : IEventHandler
{
    protected readonly ITelegramBotClient _botClient;

    internal EventHandlerBase(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public virtual Task Process(Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
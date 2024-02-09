using Telegram.Bot;
using Worker.Telegram.Services.Abstract;
using Worker.Telegram.Services.EventDispatcher;

namespace Worker.Telegram.Services;

// Compose Receiver and UpdateHandler implementation
public class ReceiverService : ReceiverServiceBase<EventDispatcherService>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        EventDispatcherService updateDispatcher,
        ILogger<ReceiverServiceBase<EventDispatcherService>> logger)
        : base(botClient, updateDispatcher, logger)
    {
    }
}
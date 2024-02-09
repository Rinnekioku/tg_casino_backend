using API.Telegram.Services.Abstract;
using API.Telegram.Services.EventDispatcher;
using Telegram.Bot;

namespace API.Telegram.Services;

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
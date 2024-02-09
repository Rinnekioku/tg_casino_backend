using Telegram.Bot;
using Worker.Telegram.Services.Abstract;
using Worker.Telegram.Services.TelegramWebhookDispatching;

namespace Worker.Telegram.Services;

// Compose Receiver and UpdateHandler implementation
public class ReceiverService : ReceiverServiceBase<TelegramWebhookDispatcher>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        TelegramWebhookDispatcher updateDispatcher,
        ILogger<ReceiverServiceBase<TelegramWebhookDispatcher>> logger)
        : base(botClient, updateDispatcher, logger)
    {
    }
}
using Telegram.Bot.Types.Enums;
using Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

namespace Worker.Telegram.Services.Interfaces;

public interface ITelegramWebhookDispatcher : global::Telegram.Bot.Polling.IUpdateHandler
{
    public void On<TH>(UpdateType updateType)
        where TH : IUpdateHandler;
}
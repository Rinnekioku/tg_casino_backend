using Telegram.Bot.Types;

namespace Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

public interface IUpdateHandler
{ 
    public Task Handle(Update update, CancellationToken cancellationToken);
}
using Common.Utils.Messaging.Events;
using Common.Utils.Messaging.Interfaces;

namespace Worker.Telegram.Messaging.Handlers;

public class TelegramLoginResponseHandler : IEventHandler<TelegramLoginResponse>
{
    private readonly ILogger<TelegramLoginResponseHandler> _logger;

    public TelegramLoginResponseHandler(ILogger<TelegramLoginResponseHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(TelegramLoginResponse @event)
    {
        await Task.Yield();
        _logger.LogInformation("User logged in via telegram with {Token} token", @event.Token);
    }
}
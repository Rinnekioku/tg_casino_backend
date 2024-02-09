using Common.Utils.Messaging.Events;
using Common.Utils.Messaging.Interfaces;

namespace API.Casino.Messaging.Handlers;

public class TelegramLoginRequestHandler : IEventHandler<TelegramLoginRequest>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<TelegramLoginRequestHandler> _logger;

    public TelegramLoginRequestHandler(ILogger<TelegramLoginRequestHandler> logger, IEventBus eventBus)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Handle(TelegramLoginRequest @event)
    {
        _logger.LogInformation("User with {Username} requested in via telegram", @event.Username);

        await _eventBus.Publish(new TelegramLoginResponse { Token = $"{@event.Username}_token" });
    }
}
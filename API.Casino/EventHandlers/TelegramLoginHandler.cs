using Common.Utils.EventBus.Events;
using Common.Utils.EventBus.Interfaces;

namespace API.Casino.EventHandlers;

public class TelegramLoginHandler : IEventHandler<TelegramLogin>
{
    private readonly ILogger<TelegramLoginHandler> _logger;

    public TelegramLoginHandler(ILogger<TelegramLoginHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(TelegramLogin @event)
    {
        await Task.Yield();

        _logger.LogInformation("User with {Username} logged in via telegram", @event.Username);
    }
}
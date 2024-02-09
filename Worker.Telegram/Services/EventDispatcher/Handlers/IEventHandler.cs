using Worker.Telegram.Enums;
using Telegram.Bot.Types;

namespace Worker.Telegram.Services.EventDispatcher.Handlers;

public interface IEventHandler
{ 
    public Task Process(Update update, CancellationToken cancellationToken);
}
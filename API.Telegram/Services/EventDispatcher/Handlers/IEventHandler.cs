using API.Telegram.Enums;
using Telegram.Bot.Types;

namespace API.Telegram.Services.EventDispatcher.Handlers;

public interface IEventHandler
{ 
    public Task Process(Update update);
}
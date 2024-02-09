namespace Common.Utils.EventBus.Events;

public class TelegramLogin : Event
{
    public string Username { get; init; } = "";
}
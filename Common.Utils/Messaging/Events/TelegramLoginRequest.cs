namespace Common.Utils.Messaging.Events;

public class TelegramLoginRequest : Event
{
    public string Username { get; init; } = "";
}
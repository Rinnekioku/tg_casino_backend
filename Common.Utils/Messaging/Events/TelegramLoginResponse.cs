namespace Common.Utils.Messaging.Events;

public class TelegramLoginResponse : Event
{
    public string Token { get; init; }
}
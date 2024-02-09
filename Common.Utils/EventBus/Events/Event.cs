namespace Common.Utils.EventBus.Events;

public abstract class Event
{
    public DateTime TimeStamp { get; protected set; }

    protected Event()
    {
        TimeStamp = DateTime.Now;
    }
}
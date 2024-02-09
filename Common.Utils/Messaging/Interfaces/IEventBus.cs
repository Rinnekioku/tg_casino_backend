using Common.Utils.Messaging.Events;

namespace Common.Utils.Messaging.Interfaces;

public interface IEventBus
{
    public Task Publish<T>(T @event) where T : Event;

    public void On<T, TH>() 
        where T : Event
        where TH : IEventHandler<T>;
}
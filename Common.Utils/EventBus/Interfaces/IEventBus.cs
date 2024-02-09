using Common.Utils.EventBus.Events;

namespace Common.Utils.EventBus.Interfaces;

public interface IEventBus
{
    public Task Publish<T>(T @event) where T : Event;

    public void On<T, TH>() 
        where T : Event
        where TH : IEventHandler<T>, new();
}
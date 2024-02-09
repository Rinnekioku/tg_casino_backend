using Common.Utils.EventBus.Events;

namespace Common.Utils.EventBus.Interfaces;

public interface IEventHandler<in T> : IEventHandler where T : Event
{
    public Task Handle(T @event);
}

public interface IEventHandler
{
}
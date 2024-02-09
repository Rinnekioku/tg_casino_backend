using Common.Utils.Messaging.Events;

namespace Common.Utils.Messaging.Interfaces;

public interface IEventHandler<in T> where T : Event
{
    public Task Handle(T @event);
}
using Chapter4.Metadata;
using System;
using System.Threading.Tasks;

namespace Chapter4.Events.Handlers
{
    public interface IEventHandler<TEvent> 
        where TEvent : Event
    {
        Task HandleAsync(TEvent @event, IReadOnlyMetadata metadata);
    }
}

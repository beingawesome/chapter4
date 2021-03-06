using Chapter4.Events;
using Chapter4.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter4.EventStore.AspNetCore
{
    internal class EventHandlerService : HostedService
    {
        private EventHandlers _events;
        private EventStoreFacade _connection;

        public EventHandlerService(EventStoreFacade connection, EventHandlers events)
        {
            _connection = connection;
            _events = events;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken) => _connection.CatchUp(HandleEvent);

        private void HandleEvent(EventDescriptor descriptor)
        {
            var handler = HandleEvent((dynamic)descriptor.Event, descriptor.Metadata);

            Task.WaitAll(handler);
        }

        private async Task HandleEvent<TEvent>(TEvent e, IReadOnlyMetadata metadata)
            where TEvent : Event
        {
            foreach(var handler in _events.Build<TEvent>())
            {
                await handler.HandleAsync(e, metadata).ConfigureAwait(false);
            }
        }        
    }
}

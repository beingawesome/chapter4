using Chapter4.Events;
using Chapter4.Events.Store;
using Chapter4.EventSourcing;
using Chapter4.EventStore;
using Chapter4.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.EventStore
{
    internal class EventStoreStore : IEventStore, IDisposable
    {
        private readonly EventMetadataFactory _metadata;
        private readonly EventStoreFacade _connection;

        public EventStoreStore(EventStoreFacade connection, EventMetadataFactory metadata)
        {
            _connection = connection;
            _metadata = metadata;
        }

        // TODO: should i to list?
        public async Task<IEnumerable<Event>> GetEventsAsync<T>(string id, long start)
            where T : AggregateRoot
        {
            var aggregate = typeof(T).FullName;

            return (await _connection.Read(aggregate, id, start).ConfigureAwait(false)).Select(x => x.Event);
        }

        public Task<Commit> SaveAsync<T>(string id, long expectedVersion, IEnumerable<Event> events)
            where T : AggregateRoot
        {
            var aggregate = typeof(T).FullName;
            var descriptors = events.Select(x => new EventDescriptor(x, _metadata.Create(x)));

            return _connection.Save(aggregate, id, expectedVersion, descriptors);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}

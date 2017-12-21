﻿using Chapter4.EventStore;
using Chapter4.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.Events.Store.EventStore
{
    internal class EventStoreStore : IEventStore, IDisposable
    {
        private readonly IEventMetadataFactory _metadata;
        private readonly EventStoreFacade _connection;

        public EventStoreStore(EventStoreFacade connection, IEventMetadataFactory metadata)
        {
            _connection = connection;
            _metadata = metadata;
        }

        // TODO: should i to list?
        public async Task<IEnumerable<Event>> GetEventsAsync(string aggregate, string id, long start)
        {
            return (await _connection.Read(aggregate, id, start).ConfigureAwait(false)).Select(x => x.Event);
        }

        public Task<Commit> SaveAsync(string aggregate, string id, long expectedVersion, IEnumerable<Event> events)
        {
            var descriptors = events.Select(x => new EventDescriptor(x, _metadata.Create(x)));

            return _connection.Save(aggregate, id, expectedVersion, descriptors);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}

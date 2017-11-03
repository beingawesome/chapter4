using Chapter4.Versioning;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.EventStore
{
    public class EventStoreFacade : IDisposable
    {
        private const int PageSize = 200;

        private EventSerializer _serializer = new EventSerializer();
        private List<IDisposable> _disposables = new List<IDisposable>();

        private Lazy<Task<IEventStoreConnection>> _connection;

        internal EventStoreFacade(ConnectionPool pool, string connectionString)
        {
            _connection = new Lazy<Task<IEventStoreConnection>>(() => pool.Get(connectionString), true);
        }

        public async Task CatchUp(Action<EventDescriptor> callback)
        {
            var connection = await EnsureConnectionAsync();

            var subscription = connection.SubscribeToAllFrom(null, CatchUpSubscriptionSettings.Default, (s, r) => OnEvent(r, callback));

            _disposables.Add(new CatchUpDisposable(subscription));
        }

        public async Task Live(Action<EventDescriptor> callback)
        {
            var connection = await EnsureConnectionAsync();

            var subscription = await connection.SubscribeToAllAsync(false, (s, r) => OnEvent(r, callback));

            _disposables.Add(subscription);
        }

        public async Task<IEnumerable<EventDescriptor>> Read(string aggregate, string id, long start)
        {
            var connection = await EnsureConnectionAsync();
            var stream = GetStream(aggregate, id);
            var result = Enumerable.Empty<EventDescriptor>();

            var slice = (StreamEventsSlice)null;
            var current = start;

            do
            {
                slice = await connection.ReadStreamEventsForwardAsync(stream, current, PageSize, false);

                current = slice.NextEventNumber;

                var events = slice.Events.Select(x => ConvertEvent(x, true)).ToList();

                result = result.Concat(events);

            } while (!slice.IsEndOfStream);

            return result;
        }

        public async Task<Commit> Save(string aggregate, string id, long expectedVersion, IEnumerable<EventDescriptor> events)
        {
            var connection = await EnsureConnectionAsync();
            var stream = GetStream(aggregate, id);
            var data = events.Select(x => _serializer.Serialize(x));

            var result = await connection.AppendToStreamAsync(stream, expectedVersion, data);

            return new Commit(result.LogPosition.CommitPosition, result.LogPosition.PreparePosition);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                try
                {
                    disposable?.Dispose();
                }
                catch
                {
                }
            }
        }

        private void OnEvent(ResolvedEvent resolved, Action<EventDescriptor> callback)
        {
            var e = ConvertEvent(resolved, false);

            if (e != null)
            {
                callback(e);
            }
        }

        private EventDescriptor ConvertEvent(ResolvedEvent resolved, bool throwIfUnknown) => _serializer.Deserialize(resolved, false);

        private async Task<IEventStoreConnection> EnsureConnectionAsync()
        {
            var connection = _connection.Value;

            if (!connection.IsCompleted)
            {
                await connection;
            }

            return connection.Result;
        }

        private string GetStream(string aggregate, string id) => $"{aggregate}-{id}";

        private class CatchUpDisposable : IDisposable
        {
            private EventStoreCatchUpSubscription _subscription;

            public CatchUpDisposable(EventStoreCatchUpSubscription subscription) => _subscription = subscription;

            public void Dispose() => _subscription?.Stop();
        }
    }
}

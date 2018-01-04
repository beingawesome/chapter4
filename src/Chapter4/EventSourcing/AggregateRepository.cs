using Chapter4.Events;
using Chapter4.Events.Store;
using Chapter4.Snapshots.Store;
using Chapter4.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.EventSourcing
{
    public class AggregateRepository
    {
        public AggregateRepository(IEventStore events, ISnapshotStore snapshots)
        {
            Events = events;
            Snapshots = snapshots;
        }

        protected IEventStore Events { get; }
        protected ISnapshotStore Snapshots { get; }
        private AggregateBuilder Builder { get; } = new AggregateBuilder();

        public virtual async Task<T> GetByIdAsync<T>(string id)
            where T : AggregateRoot
        {
            var type = typeof(T).FullName;
            var snapshot = await Snapshots.GetLatestSnapshot(type, id).ConfigureAwait(false);
            var version = snapshot?.Version ?? AggregateRoot.InitialVersion;

            var events = await Events.GetEventsAsync(type, id, version + 1).ConfigureAwait(false) ?? Enumerable.Empty<Event>();

            if (snapshot == null && events.IsNullOrEmpty())
            {
                return null;
            }

            return await Builder.BuildAsync<T>(id, version, snapshot?.Snapshot, events).ConfigureAwait(false);
        }

        public virtual async Task<Commit> SaveAsync<T>(T aggregate)
            where T : AggregateRoot
        {
            var version = await Events.SaveAsync(typeof(T).FullName, aggregate.Id, aggregate.Proxy.Version, aggregate.Proxy.Uncommitted).ConfigureAwait(false);

            aggregate.Proxy.Commit();

            return version;
        }
    }
}

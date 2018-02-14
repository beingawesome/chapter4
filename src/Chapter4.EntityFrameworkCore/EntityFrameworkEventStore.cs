using Chapter4.Events;
using Chapter4.Events.Store;
using Chapter4.EventSourcing;
using Chapter4.Versioning;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.EntityFrameworkCore
{
    internal class EntityFrameworkEventStore : IEventStore
    {
        private readonly EventSerializer _serializer = new EventSerializer();

        private readonly DbContext _context;
        private readonly EventMetadataFactory _metadata;
        private readonly ModelUpdaterFactory _modelUpdaters;

        public EntityFrameworkEventStore(DbContext context, EventMetadataFactory metadata, ModelUpdaterFactory modelUpdaters)
        {
            _context = context;
            _metadata = metadata;
            _modelUpdaters = modelUpdaters;
        }

        public Task<IEnumerable<Event>> GetEventsAsync<T>(string aggregate, string id, long start) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }

        public async Task<Commit> SaveAsync<T>(string aggregate, string id, long expectedVersion, IEnumerable<Event> events) where T : AggregateRoot
        {
            var version = expectedVersion;
            var updater = _modelUpdaters.Get<T>();

            var descriptors = events.Select(x => new EventDescriptor(aggregate, id, ++version, x, _metadata.Create(x))).ToList();
            var data = descriptors.Select(x => _serializer.Serialize(x)).ToList();

            // TODO: Replace with transaction scope
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                var expected = await _context.Set<EventData>()
                                                    .Where(x => x.Aggregate == aggregate && x.AggregateId == id)
                                                    .OrderByDescending(x => x.Version)
                                                    .AsNoTracking()
                                                    .LastOrDefaultAsync();

                var current = expected?.Version ?? -1;

                if (current != expectedVersion) throw new ConcurrencyException(current, expectedVersion);

                await _context.AddRangeAsync(data);

                await updater.InitializeAsync(id);

                foreach (var descriptor in descriptors)
                {
                    updater.Apply(descriptor.Event, descriptor.Metadata);
                }

                await _context.SaveChangesAsync();

                trans.Commit();
            }

            return Commit.Any;
        }
    }
}

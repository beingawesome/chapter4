using Chapter4.EventSourcing;
using Chapter4.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chapter4.Events.Store
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> GetEventsAsync<T>(string aggregate, string id, long start) where T : AggregateRoot;
        Task<Commit> SaveAsync<T>(string aggregate, string id, long expectedVersion, IEnumerable<Event> events) where T : AggregateRoot;
    }
}

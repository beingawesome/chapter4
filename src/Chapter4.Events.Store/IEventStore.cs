using Chapter4.Versioning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chapter4.Events.Store
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> GetEventsAsync(string aggregate, string id, long start);
        Task<Commit> SaveAsync(string aggregate, string id, long expectedVersion, IEnumerable<Event> events);
    }
}

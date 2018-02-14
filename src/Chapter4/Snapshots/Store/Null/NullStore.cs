using System;
using System.Threading.Tasks;
using Chapter4.EventSourcing;

namespace Chapter4.Snapshots.Store.Null
{
    internal sealed class NullStore : ISnapshotStore
    {
        public Task<SnapshotDescriptor> GetLatestSnapshot<T>(string id) where T : AggregateRoot
        {
            return Task.FromResult<SnapshotDescriptor>(null);
        }        

        public Task SaveAsync<T>(string id, SnapshotDescriptor snapshot) where T : AggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}

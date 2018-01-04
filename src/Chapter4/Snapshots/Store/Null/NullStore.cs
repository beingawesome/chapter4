using System;
using System.Threading.Tasks;

namespace Chapter4.Snapshots.Store.Null
{
    internal sealed class NullStore : ISnapshotStore
    {
        public Task<SnapshotDescriptor> GetLatestSnapshot(string type, string id)
        {
            return Task.FromResult<SnapshotDescriptor>(null);
        }

        public Task SaveAsync(string type, string id, SnapshotDescriptor snapshot)
        {
            throw new NotSupportedException();
        }
    }
}

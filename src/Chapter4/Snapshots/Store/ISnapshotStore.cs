using System;
using System.Threading.Tasks;

namespace Chapter4.Snapshots.Store
{
    public interface ISnapshotStore
    {
        Task<SnapshotDescriptor> GetLatestSnapshot(string type, string id);
        Task SaveAsync(string type, string id, SnapshotDescriptor snapshot);
    }
}

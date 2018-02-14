using Chapter4.EventSourcing;
using System;
using System.Threading.Tasks;

namespace Chapter4.Snapshots.Store
{
    public interface ISnapshotStore
    {
        Task<SnapshotDescriptor> GetLatestSnapshot<T>(string id) where T : AggregateRoot;
        Task SaveAsync<T>(string id, SnapshotDescriptor snapshot) where T: AggregateRoot;
    }
}

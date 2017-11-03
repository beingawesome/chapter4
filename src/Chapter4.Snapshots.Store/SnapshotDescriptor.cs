namespace Chapter4.Snapshots.Store
{
    public sealed class SnapshotDescriptor
    {
        public SnapshotDescriptor(string type, string id, long version, Snapshot snapshot)
        {
            Type = type;
            Id = id;
            Version = version;
            Snapshot = snapshot;
        }

        public string Type { get; }
        public string Id { get; }
        public long Version { get; }
        public Snapshot Snapshot { get; }
    }
}
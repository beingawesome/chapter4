namespace Chapter4.Metadata
{
    public interface IMetadata : IReadOnlyMetadata
    {
        void Set<TMetadata>(TMetadata metadata);

        IMetadata Clone();
    }
}

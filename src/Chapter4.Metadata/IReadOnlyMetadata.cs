namespace Chapter4.Metadata
{
    public interface IReadOnlyMetadata
    {
        TMetadata Get<TMetadata>();
    }
}

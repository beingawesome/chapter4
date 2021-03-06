using Chapter4.Metadata;

namespace Chapter4.Commands.Messaging
{
    public interface ICommandMetadataFactory
    {
        IMetadata Create<TCommand>(TCommand command)
            where TCommand : Command;
    }
}

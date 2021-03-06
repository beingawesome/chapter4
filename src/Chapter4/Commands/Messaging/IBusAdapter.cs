using Chapter4.Metadata;
using Chapter4.Versioning;
using System.Threading.Tasks;

namespace Chapter4.Commands.Messaging
{
    public interface IBusAdapter
    {
        Task<CommandResult> SendAsync<TCommand>(TCommand command, IReadOnlyMetadata metadata)
            where TCommand : Command;
    }
}

using Chapter4.Metadata;
using Chapter4.Versioning;
using System;
using System.Threading.Tasks;

namespace Chapter4.Commands.Handlers
{
    public interface ICommandHandler<TCommand>
        where TCommand : Command
    {
        Task<CommandResult> HandleAsync(TCommand command, IReadOnlyMetadata metadata);
    }
}

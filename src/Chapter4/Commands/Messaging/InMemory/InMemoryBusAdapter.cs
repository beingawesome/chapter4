using Chapter4.Metadata;
using Chapter4.Versioning;
using System.Threading.Tasks;

namespace Chapter4.Commands.Messaging
{
    internal class InMemoryBusAdapter : IBusAdapter
    {
        private readonly CommandHandlers _handlers;

        public InMemoryBusAdapter(CommandHandlers handlers)
        {
            _handlers = handlers;
        }

        public Task<CommandResult> SendAsync<TCommand>(TCommand command, IReadOnlyMetadata metadata)
            where TCommand : Command
        {
            var handler = _handlers.Build<TCommand>();            

            return handler.HandleAsync(command, metadata);
        }
    }
}

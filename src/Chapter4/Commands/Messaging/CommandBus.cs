using Chapter4.Retry;
using Chapter4.Versioning;
using System.Threading.Tasks;

namespace Chapter4.Commands.Messaging
{
    public sealed class CommandBus
    {
        private readonly IBusAdapter _bus;
        private readonly ICommandMetadataFactory _metadata;
        private readonly CommandMetadataAccessor _accessor;
        private readonly IRetryAlgorithm _retry;

        internal CommandBus(IBusAdapter bus, ICommandMetadataFactory metadata, CommandMetadataAccessor accessor, IRetryAlgorithm retry)
        {
            _bus = bus;
            _metadata = metadata;
            _accessor = accessor;
            _retry = retry;
        }

        public async Task<CommandResult> SendAsync<TCommand>(TCommand command)
            where TCommand : Command
        {
            var meta = _metadata.Create(command);

            using (_accessor.SetMetadata(meta))
            {
                return await _retry.Execute(() => _bus.SendAsync(command, meta)).ConfigureAwait(false);
            }
        }
    }
}

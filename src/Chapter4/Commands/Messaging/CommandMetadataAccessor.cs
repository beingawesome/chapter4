using Chapter4.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Chapter4.Commands.Messaging
{
    public class CommandMetadataAccessor
    {
        private readonly AsyncLocal<IMetadata> _local;

        internal CommandMetadataAccessor() => _local = new AsyncLocal<IMetadata>();

        public IMetadata Metadata => _local.Value;

        internal IDisposable SetMetadata(IMetadata metadata)
        {
            var current = _local.Value;

            _local.Value = metadata;

            return new ActionDisposable(() => _local.Value = current);
        }

        private class ActionDisposable : IDisposable
        {
            private readonly Action _onDispose;

            public ActionDisposable(Action onDispose) => _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));

            public void Dispose() => _onDispose();
        }
    }
}

using Chapter4.Commands.Messaging;
using Chapter4.Events;
using Chapter4.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.EntityFrameworkCore
{
    internal class EventMetadataFactory
    {
        private readonly CommandMetadataAccessor _accessor;

        public EventMetadataFactory(CommandMetadataAccessor accessor) => _accessor = accessor;

        public IMetadata Create(Event e) => _accessor.Metadata?.Clone();
    }
}

using Chapter4.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.Events.Store.EventStore
{
    public interface IEventMetadataFactory
    {
        IMetadata Create(Event e);
    }
}

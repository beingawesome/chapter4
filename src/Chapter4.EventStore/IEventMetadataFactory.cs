using Chapter4.Events;
using Chapter4.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.EventStore
{
    public interface IEventMetadataFactory
    {
        IMetadata Create(Event e);
    }
}

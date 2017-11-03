using Chapter4.Events;
using Chapter4.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.EventStore
{
    public class EventDescriptor
    {
        public EventDescriptor(Event e, IMetadata metadata)
        {
            Event = e;
            Metadata = metadata;
        }

        public Event Event { get; }
        public IMetadata Metadata { get; }
    }
}

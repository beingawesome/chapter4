using Chapter4.Events;
using Chapter4.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.EntityFrameworkCore
{
    internal class EventDescriptor
    {
        public EventDescriptor(string aggregate, string aggregateId, long version, Event e, IMetadata metadata)
        {
            Aggregate = aggregate;
            AggregateId = aggregateId;
            Version = version;

            Event = e;
            Metadata = metadata;
        }

        public string Aggregate { get; }
        public string AggregateId { get; }
        public long Version { get; }

        public Event Event { get; }
        public IMetadata Metadata { get; }
    }
}

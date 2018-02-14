using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.EntityFrameworkCore
{
    public class EventData
    {
        internal EventData(string aggregate, string aggregateId, long version, string data, string metadata)
        {
            Aggregate = aggregate;
            AggregateId = aggregateId;
            Version = version;
            Data = data;
            Metadata = metadata;
        }

        private EventData() { }

        public long Id { get; private set; }

        public string Aggregate { get; set; }
        public string AggregateId { get; set; }
        public long Version { get; private set; }
        public string Data { get; private set; }
        public string Metadata { get; private set; }
    }

}

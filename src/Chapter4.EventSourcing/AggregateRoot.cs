using Chapter4.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4.EventSourcing
{
    public abstract class AggregateRoot
    {
        internal static readonly long InitialVersion = -1;

        protected AggregateRoot(string id)
        {
            Id = id;
            Proxy = new AggregateProxy(this);
        }

        public string Id { get; }

        internal AggregateProxy Proxy { get; }

        protected void Emit(Event @event)
        {
            Proxy.Push(@event);
        }
    }
}

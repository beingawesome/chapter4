using Chapter4.Events;
using Chapter4.Snapshots;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chapter4.EventSourcing
{
    internal class AggregateProxy
    {
        private static TypeCache<Event> EventHandlers = new TypeCache<Event>();
        private static TypeCache<Snapshot> SnapshotHandlers = new TypeCache<Snapshot>();
        private static Type EventType = typeof(Event);

        private LinkedList<Event> _uncommitted = new LinkedList<Event>();

        public AggregateProxy(AggregateRoot aggregate)
        {
            Aggregate = aggregate;
            Version = AggregateRoot.InitialVersion;
        }

        public long Version { get; private set; }
        protected AggregateRoot Aggregate { get; }
        protected Type AggregateType { get; }

        public void Apply(Snapshot snapshot, long version)
        {
            if (Version > AggregateRoot.InitialVersion) throw new ConcurrencyException(Version, AggregateRoot.InitialVersion);

            Version = version;
            Apply(snapshot);
        }

        public void Push(Event @event)
        {
            Apply(@event);

            _uncommitted.AddLast(@event);
        }

        public IEnumerable<Event> Uncommitted => _uncommitted;

        public void Commit()
        {
            Version += _uncommitted.Count;
            _uncommitted.Clear();
        }

        private void Apply(Event @event) => EventHandlers.GetHandler(Aggregate.GetType(), @event.GetType())?.Invoke(Aggregate, new object[] { @event });

        private void Apply(Snapshot snapshot) => SnapshotHandlers.GetHandler(Aggregate.GetType(), snapshot.GetType())?.Invoke(Aggregate, new object[] { snapshot });

        private class TypeCache<T> : ConcurrentDictionary<Type, HandlerCache>
        {
            private Type Interface = typeof(T);

            public MethodInfo GetHandler(Type aggregate, Type target)
            {
                var handlers = this.GetOrAdd(aggregate, BuildHandlerLookup);

                if (handlers.TryGetValue(target, out MethodInfo handler))
                {
                    return handler;
                }

                return null;
            }

            private HandlerCache BuildHandlerLookup(Type aggregate)
            {
                var result = new HandlerCache();

                aggregate.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                         .Where(x => x.Name == nameof(AggregateProxy.Apply))
                         .Select(x => new { Method = x, Params = x.GetParameters() })
                         .Where(x => x.Params.Length == 1 && Interface.IsAssignableFrom(x.Params[0].ParameterType))
                         .ToList()
                         .ForEach(x => result[x.Params[0].ParameterType] = x.Method);

                return result;
            }
        }

        private class HandlerCache : Dictionary<Type, MethodInfo> { }
    }
}

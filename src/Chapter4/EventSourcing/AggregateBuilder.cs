using Chapter4.Events;
using Chapter4.Snapshots;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Chapter4.EventSourcing
{
    internal class AggregateBuilder
    {
        private static CtorCache Ctors = new CtorCache();
        private static Type IdType = typeof(string);

        public async Task<T> BuildAsync<T>(string id, long version, Snapshot snapshot, IEnumerable<Event> events) where T : AggregateRoot
        {
            var aggregate = Create<T>(id);

            if (snapshot != null)
            {
                aggregate.Proxy.Apply(snapshot, version);
            }

            foreach (var @event in events)
            {
                aggregate.Proxy.Push(@event);
            }

            aggregate.Proxy.Commit();

            return await Task.FromResult(aggregate);
        }

        private T Create<T>(string id) where T : AggregateRoot => (T)Ctors.GetOrAdd(typeof(T)).Invoke(new object[] { id });

        private class CtorCache : ConcurrentDictionary<Type, ConstructorInfo>
        {
            public ConstructorInfo GetOrAdd(Type type) => this.GetOrAdd(type, GetCtor);

            private ConstructorInfo GetCtor(Type type) => type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                                                              .Select(x => new { Params = x.GetParameters(), Ctor = x })
                                                              .Where(x => x.Params.Length == 1 && x.Params[0].ParameterType == IdType)
                                                              .Select(x => x.Ctor)
                                                              .Single();
        }
    }
}

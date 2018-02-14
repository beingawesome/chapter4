using Chapter4.Events;
using Chapter4.EventSourcing;
using Chapter4.Metadata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.EntityFrameworkCore
{
    public abstract class ModelUpdater<T> where T : AggregateRoot
    {
        private static TypeCache<Event> EventHandlers = new TypeCache<Event>();

        private static readonly ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();

        public abstract Task InitializeAsync(string id);

        internal void Apply(Event e, IReadOnlyMetadata metadata)
        {
            EventHandlers.GetHandler(typeof(T), e.GetType()).Invoke(this, new object[] { e, metadata });

            ApplyAll(metadata);
        }

        protected virtual void ApplyAll(IReadOnlyMetadata metadata) { }

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

                throw new Exception("Event Handler not found.");
            }

            private HandlerCache BuildHandlerLookup(Type aggregate)
            {
                var result = new HandlerCache();

                aggregate.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                         .Where(x => x.Name == nameof(Apply))
                         .Select(x => new { Method = x, Params = x.GetParameters() })
                         .Where(x => x.Params.Length == 2 && Interface.IsAssignableFrom(x.Params[0].ParameterType) && x.Params[1].ParameterType == typeof(IReadOnlyMetadata))
                         .ToList()
                         .ForEach(x => result[x.Params[0].ParameterType] = x.Method);

                return result;
            }
        }

        private class HandlerCache : Dictionary<Type, MethodInfo> { }
    }
}

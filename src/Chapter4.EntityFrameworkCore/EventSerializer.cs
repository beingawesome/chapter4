using Chapter4.Events;
using Chapter4.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using DynamicMetdata = Chapter4.Metadata.Dynamic.Metadata;

namespace Chapter4.EntityFrameworkCore
{
    internal class EventSerializer
    {
        private const string TypeProperty = "$type";
        private const string VersionProperty = "$version";

        public EventData Serialize(EventDescriptor e)
        {
            var name = e.Event.GetType().Name;

            var id = Guid.NewGuid();
            var data = SerializeEvent(e.Event);
            var metadata = SerializeMetadata(e.Metadata);

            return new EventData(e.Aggregate, e.AggregateId, e.Version, data, metadata);
        }

        public EventDescriptor Deserialize(EventData @event, bool throwOnError)
        {
            var metadata = DeserializeMetadata(@event.Metadata);
            var e = DeserializeEvent(@event.Data, throwOnError);

            return e == null
                ? null
                : new EventDescriptor(@event.Aggregate, @event.AggregateId, @event.Version, e, metadata);
        }

        private string SerializeEvent(Event e)
        {
            var obj = JObject.FromObject(e);

            // TODO: Security risk!!! Don't do in production!!!
            obj.AddFirst(new JProperty(VersionProperty, "1.0.0"));
            obj.AddFirst(new JProperty(TypeProperty, e.GetType().AssemblyQualifiedName));

            return JsonConvert.SerializeObject(obj);
        }

        private Event DeserializeEvent(string json, bool throwOnError)
        {
            try
            {
                var obj = JObject.Parse(json);

                if (!obj.TryGetValue(TypeProperty, out var prop)) return default(Event);

                // TODO: Security risk!!! Don't do in production!!!
                var type = Type.GetType(prop.Value<string>(), false);

                if (type == null) return default(Event);

                if (!typeof(Event).IsAssignableFrom(type))
                {
                    return throwOnError
                            ? throw new Exception("Event is not of type Event")
                            : default(Event);
                }

                return (Event)obj.ToObject(type);
            }
            catch
            {
                return default(Event);
            }
        }

        private string SerializeMetadata(IMetadata metadata) => JsonConvert.SerializeObject(metadata);

        private IMetadata DeserializeMetadata(string json) => JsonConvert.DeserializeObject<DynamicMetdata>(value: json);
    }
}

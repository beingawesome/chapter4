using Chapter4.Events;
using Chapter4.Metadata;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using DynamicMetdata = Chapter4.Metadata.Dynamic.Metadata;

namespace Chapter4.EventStore
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

            return new EventData(id, name, true, data, metadata);
        }

        public EventDescriptor Deserialize(ResolvedEvent resolved, bool throwOnError)
        {
            var metadata = DeserializeMetadata(resolved.Event.Metadata);

            var e = DeserializeEvent(resolved.Event.Data, throwOnError);

            return e == null
                ? null
                : new EventDescriptor(e, metadata);
        }

        private byte[] SerializeEvent(Event e)
        {
            var obj = JObject.FromObject(e);

            // TODO: Security risk!!! Don't do in production!!!
            obj.AddFirst(new JProperty(VersionProperty, "1.0.0"));
            obj.AddFirst(new JProperty(TypeProperty, e.GetType().AssemblyQualifiedName));

            var json = JsonConvert.SerializeObject(obj);

            return Encoding.UTF8.GetBytes(json);
        }

        private Event DeserializeEvent(byte[] bytes, bool throwOnError)
        {
            try
            {
                var json = Encoding.UTF8.GetString(bytes);

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

        private byte[] SerializeMetadata(IMetadata metadata)
        {
            var json = JsonConvert.SerializeObject(metadata);

            return Encoding.UTF8.GetBytes(json);
        }

        private IMetadata DeserializeMetadata(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<DynamicMetdata>(json);
        }
    }
}

using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Chapter4.Metadata.Dynamic
{
    public class Metadata : Dictionary<string, JObject>, IMetadata
    {
        public IMetadata Clone()
        {
            var clone = new Metadata();

            foreach(var item in this)
            {
                clone.Add(item.Key, item.Value);
            }

            return clone;
        }

        public TMetadata Get<TMetadata>()
        {
            var key = typeof(TMetadata).Name;

            return TryGetValue(key, out var value)
                        ? value.ToObject<TMetadata>()
                        : default(TMetadata);
        }

        public void Set<TMetadata>(TMetadata feature)
        {
            var key = typeof(TMetadata).Name;

            this[key] = JObject.FromObject(feature);
        }
    }
}

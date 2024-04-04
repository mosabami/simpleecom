

using Newtonsoft.Json;

namespace Simpleecom.Shared.Models
{
    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("type")]
        public string Type { get; set; }

        public Item() => Type = GetType().Name;

        protected virtual string GetPartitionKeyValue() => Id;

    }
}

using Newtonsoft.Json;

namespace Simpleecom.Shared.Models
{
    public class Item
    {
        private string _id;

        [JsonProperty("id")]
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    _id = Guid.NewGuid().ToString();
                }
                return _id;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _id = Guid.NewGuid().ToString();
                }
                else
                {
                    _id = value;
                }
            }
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        public Item() => Type = GetType().Name;
    }
}

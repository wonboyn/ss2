using Newtonsoft.Json;


namespace SelfServiceProj
{
    public class Action
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "boturl")]
        public string BotUrl { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "docurl")]
        public string DocUrl { get; set; } = string.Empty;
    }
}
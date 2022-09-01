using Newtonsoft.Json;


namespace SelfServiceProj
{
    public class Action
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "boturl")]
        public string BotUrl { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "docurl")]
        public string DocUrl { get; set; } = string.Empty;
    }
}
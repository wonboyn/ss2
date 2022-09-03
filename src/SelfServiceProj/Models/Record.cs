using Newtonsoft.Json;


namespace SelfServiceProj
{
    public class Record
    {
        [JsonProperty(PropertyName = "record")]
        public Action Item { get; set; } = new Action();
    }
}
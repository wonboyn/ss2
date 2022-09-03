using Newtonsoft.Json;


namespace SelfServiceProj
{
    public class Records
    {
        [JsonProperty(PropertyName = "records")]
        public List<Action> ItemList { get; set; } = new List<Action>();
    }
}
using Newtonsoft.Json;


namespace SelfServiceProj
{
    public class Actions
    {
        [JsonProperty(PropertyName = "actions")]
        public List<Action> ActionList { get; set; } = new List<Action>();
    }
}
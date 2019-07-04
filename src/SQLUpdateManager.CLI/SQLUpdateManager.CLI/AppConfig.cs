using Newtonsoft.Json;

namespace SQLUpdateManager.CLI
{
    public class AppConfig
    {
        [JsonProperty("sessionLifeTime")]
        public int SessionLifeTime { get; set; }
    }
}

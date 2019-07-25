using Newtonsoft.Json;

namespace SQLUpdateManager.CLI
{
    public class AppConfig
    {
        [JsonProperty("fileEncoding")]
        public string FileEncoding { get; set; }
    }
}

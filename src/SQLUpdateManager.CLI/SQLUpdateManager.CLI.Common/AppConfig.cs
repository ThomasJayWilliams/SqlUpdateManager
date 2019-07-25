using Newtonsoft.Json;

namespace SQLUpdateManager.CLI.Common
{
    public class AppConfig
    {
        [JsonProperty("fileEncoding")]
        public string FileEncoding { get; set; }
    }
}
